using System.Collections.Generic;
using System.Linq;
using ToyBox.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


namespace ToyBox.Managers
{
    public class BuildsManager : MonoBehaviour
    {
        public static BuildsManager Instance { get; private set; }

        PlayerManager _playerManager => PlayerManager.Instance;

        [FormerlySerializedAs("objectsStruct")] [SerializeField] List<PlaceableStruct> _4objectsStruct = new();

        [FormerlySerializedAs("objects")] public List<BuildObject> Objects = new();


        int _picked = 0;
        int _turnNumber = 0;
        List<BuildObject> _objectsList = new();

        public bool selecting = false;

        public UnityEvent ObjectPlaced;

        private void Awake()
        {
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
            }
            else
            {
                build.Place(true);
                Objects.Add(build);
                ObjectPlaced.Invoke();
            }

            if (_playerManager.DoesAllPlayersFinishedBuilding())
            {
                GameModeManager.Instance.StartCountDown(3.5f);
            }

        }

        public void Shuffle(int amount)
        {
            if (_turnNumber <= 10)
            {
                _turnNumber++;
            }
            selecting = true;
            amount = _playerManager.Players.Count + 3;

            ChooseBox.Instance.gameObject.SetActive(true);

            for (int i = 0; i < amount; i++)
            {
                float probability = 0;
                GameObject chosenObject = _4objectsStruct[0]._objectPrefab;
                for (int j = 0; j < _4objectsStruct.Count; j++)
                {

                    float objectProbability = Random.Range(0, _4objectsStruct.Count*.1f)*_4objectsStruct[j]._curve.Evaluate(_turnNumber*.1f);
                    if (probability <= objectProbability)
                    {
                        probability = objectProbability;
                        chosenObject = _4objectsStruct[j]._objectPrefab;
                    }
                    Debug.Log($"Object : {_4objectsStruct[j]._objectPrefab.name} with probability : {objectProbability}");

                }
                
                GameObject go = Instantiate(chosenObject, new(Random.Range(ChooseBox.Instance.BL.position.x, ChooseBox.Instance.TR.position.x), Random.Range(ChooseBox.Instance.BL.position.y, ChooseBox.Instance.TR.position.y)), Quaternion.identity, ChooseBox.Instance.transform);
                BuildObject b = go.GetComponent<BuildObject>();

                _objectsList.Add(b);
                b.OnPickedEvent.AddListener(ObjectPicked);
            }
        }

        public void ObjectPicked()
        {
            _picked++;
            if (_picked == _playerManager.Players.Count)
            {
                ChooseBox.Instance.gameObject.SetActive(false);

                _picked = 0;

                foreach (BuildObject b in _objectsList)
                {
                    if(!b.IsChosen) Destroy(b.gameObject);
                }

                _objectsList.Clear();
                
                selecting = false;
            }
            ChooseBox.Instance.gameObject.SetActive(false);

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
    public struct PlaceableStruct
    {
        public GameObject _objectPrefab;
        public AnimationCurve _curve;

    }
}