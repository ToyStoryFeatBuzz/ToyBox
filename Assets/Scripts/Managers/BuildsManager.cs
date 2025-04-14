using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.Build;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace ToyBox.Managers
{
    public class BuildsManager : MonoBehaviour
    {
        public static BuildsManager Instance { get; private set; }

        PlayerManager _playerManager => PlayerManager.Instance;

        [SerializeField] List<StPlaceable> _objectsStruct = new();

        public List<BuildObject> Objects = new();

        ChooseBox _chooseBox => ChooseBox.Instance;

        int _picked;
        int _turnNumber;
        List<BuildObject> _objectsList = new();
        
        public bool IsSelecting;

        public Action OnObjectPlaced;

        private void Awake()
        {
            _chooseBox.gameObject.SetActive(false);
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddObject(BuildObject build) {
            
            if (build.DoErase)
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    BuildObject buildObject = Objects[i];
                    bool toDelete = false;

                    foreach (Vector2 offset in buildObject.Offsets) {
                        if (!build.ContainsPos((Vector2)buildObject.transform.position + offset)) {
                            continue;
                        }
                        toDelete = true;
                        break;
                    }

                    if (!toDelete) {
                        continue;
                    }
                    Objects.RemoveAt(i);
                    Destroy(buildObject.gameObject);
                    i--;
                }

                Destroy(build.gameObject);

                return;
            }
            build.Place(true);
            Objects.Add(build);
            OnObjectPlaced?.Invoke();
        }
        


        public void Shuffle(int amount)
        {
            if (_turnNumber <= 10)
            {
                _turnNumber++;
            }
            IsSelecting = true;
            amount = _playerManager.Players.Count + 3;

            _chooseBox.gameObject.SetActive(true);

            for (int i = 0; i < amount; i++)
            {
                float probability = 0;
                GameObject chosenObject = _objectsStruct[0].ObjectPrefab;
                for (int j = 0; j < _objectsStruct.Count; j++)
                {

                    float objectProbability = Random.Range(0, _objectsStruct.Count*.1f)*_objectsStruct[j].Curve.Evaluate(_turnNumber*.1f);
                    if (probability <= objectProbability)
                    {
                        probability = objectProbability;
                        chosenObject = _objectsStruct[j].ObjectPrefab;
                    }
                    Debug.Log($"Object : {_objectsStruct[j].ObjectPrefab.name} with probability : {objectProbability}");

                }
                
                
                GameObject go = Instantiate(chosenObject, new(Random.Range(_chooseBox.BL.position.x, _chooseBox.TR.position.x), Random.Range(_chooseBox.BL.position.y, _chooseBox.TR.position.y)), Quaternion.identity, _chooseBox.transform);
                BuildObject b = go.GetComponent<BuildObject>();
                
                _objectsList.Add(b);
                b.OnPickedEvent.AddListener(ObjectPicked);
            }
        }

        public void ObjectPicked() {
            _picked++;
            if (_picked == _playerManager.Players.Count) {
                _chooseBox.gameObject.SetActive(false);

                _picked = 0;

                foreach (BuildObject b in _objectsList.Where(b => !b.IsChosen)) {
                    Destroy(b.gameObject);
                }

                _objectsList.Clear();
                
                IsSelecting = false;
            }
            _chooseBox.gameObject.SetActive(false);

            _picked = 0;

            foreach (BuildObject buildObject in _objectsList.Where(b => !b.IsChosen)) {
                Destroy(buildObject.gameObject);
            }

            _objectsList.Clear();
                
            IsSelecting = false;
        }

        public bool CanPlace(BuildObject testedBuild) {
            return !(from build in Objects from offset in build.Offsets where testedBuild.ContainsPos((Vector2)build.transform.position + offset) select build).Any();
        }
    }
    
    [Serializable]
    public struct StPlaceable {
        public GameObject ObjectPrefab;
        public AnimationCurve Curve;

    }
}