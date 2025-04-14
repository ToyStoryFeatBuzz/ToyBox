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

        [FormerlySerializedAs("objectsStruct")] [SerializeField] List<StPlaceable> _objectsStruct = new();

        [FormerlySerializedAs("objects")] public List<BuildObject> Objects = new();

        [Header("ChooseBox")]
        [FormerlySerializedAs("objectsBox")] [SerializeField] GameObject _objectsBox;
        [FormerlySerializedAs("topRight")] [SerializeField] Transform _topRight;
        [FormerlySerializedAs("bottomLeft")] [SerializeField] Transform _bottomLeft;

        int _picked = 0;
        int _turnNumber = 0;
        List<BuildObject> _objectsList = new();

        public bool selecting = false;

        public Action OnObjectPlaced;

        private void Awake()
        {
            _objectsBox.SetActive(false);
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
            selecting = true;
            amount = _playerManager.Players.Count + 3;

            _objectsBox.SetActive(true);

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
                
                
                GameObject go = Instantiate(chosenObject, new(Random.Range(_bottomLeft.position.x, _topRight.position.x), Random.Range(_bottomLeft.position.y, _topRight.position.y)), Quaternion.identity);
                BuildObject b = go.GetComponent<BuildObject>();
                
                _objectsList.Add(b);
                b.OnPickedEvent.AddListener(ObjectPicked);
            }
        }

        public void ObjectPicked()
        {
            _picked++;
            if (_picked != _playerManager.Players.Count) {
                return;
            }
            _objectsBox.SetActive(false);

            _picked = 0;

            foreach (BuildObject buildObject in _objectsList.Where(b => !b.IsChosen)) {
                Destroy(buildObject.gameObject);
            }

            _objectsList.Clear();
                
            selecting = false;
        }

        public bool CanPlace(BuildObject testedBuild) {
            return !(from build in Objects from offset in build.Offsets where testedBuild.ContainsPos((Vector2)build.transform.position + offset) select build).Any();
        }
    }
    [System.Serializable]
    public struct StPlaceable
    {
        [FormerlySerializedAs("_objectPrefab")] public GameObject ObjectPrefab;
        [FormerlySerializedAs("_curve")] public AnimationCurve Curve;

    }
}