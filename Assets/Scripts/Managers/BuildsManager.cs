using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.Build;
using UnityEngine;
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

        private int _shuffleAmount;
        
        private void Awake(){
            if(_chooseBox !=null)
            {
                _chooseBox.gameObject.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
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
        
        public void AddObject(BuildObject build) { // Called when player placed an object
            
            if (build.DoErase) // Bombes that delete overed objects
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

                Instantiate(build._bombVisualPrefab, build.transform.position, Quaternion.identity);
                Destroy(build.gameObject);
            }
            else // Place object on map
            {
                build.Place(true);
                Objects.Add(build);
                OnObjectPlaced?.Invoke();
            }

            if (_playerManager.DoesAllPlayersFinishedBuilding()) // Start Race countdown if all players placed their objects
            {
                GameModeManager.Instance.StartCountDown(3.5f);
            }
        }
        


        public void Shuffle() // Create and place items in the choosing box
        {
            if (_turnNumber <= 10)
            {
                _turnNumber++;
            }
            IsSelecting = true;
            _shuffleAmount = _playerManager.Players.Count + 3;

            _chooseBox.gameObject.SetActive(true);
            _chooseBox.OpenChooseBox();

        }

        
        public void SpawnItem() {
            
            for (int i = 0; i < _shuffleAmount; i++)
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
                }
                
                
                GameObject go = Instantiate(chosenObject, new(Random.Range(_chooseBox.BL.position.x, _chooseBox.TR.position.x), Random.Range(_chooseBox.BL.position.y, _chooseBox.TR.position.y)), Quaternion.identity, _chooseBox.transform);
                BuildObject b = go.GetComponent<BuildObject>();
                
                _objectsList.Add(b);
                b.OnPickedEvent.AddListener(ObjectPicked);
            }
        }
        
        public void ObjectPicked() // Called when a player picked an object
        {
            _picked++;
            if (_picked == _playerManager.Players.Count) // Close choosing box when everyone has picked
            {
                _chooseBox.gameObject.SetActive(false);

                _picked = 0;

                foreach (BuildObject b in _objectsList.Where(b => !b.IsChosen)) {
                    Destroy(b.gameObject);
                }

                _objectsList.Clear();
                
                IsSelecting = false;
            }
        }

        public bool CanPlace(BuildObject testedBuild)  // Tells if testedBuild can be placed and does not collide with already placed objects
        {
            foreach (Vector2 offset in testedBuild.Offsets)
            {
                //Debug.Log("Offset " + offset + "is :" + TilemapManager.Instance.tileMapPolygon.OverlapPoint(offset+(Vector2)testedBuild.transform.position));
                if (!TilemapManager.Instance.tileMapPolygon.OverlapPoint((Vector2)testedBuild.transform.position + offset))
                {
                    return false;
                }
            }
            return !((from build in Objects
                         from offset in build.Offsets
                         where testedBuild.ContainsPos((Vector2)build.transform.position + offset)
                         select build).Any() ||
                     (from t in TilemapManager.tilesPos where testedBuild.ContainsPos(t) select t).Any());
        }

        public void ClearObjects() // Clear all placed objects
        {
            Objects.Clear();
        }
    }
    
    [Serializable]
    public struct StPlaceable {
        public GameObject ObjectPrefab;
        public AnimationCurve Curve;

    }
}