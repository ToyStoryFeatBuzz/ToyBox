using System.Collections.Generic;
using UnityEngine;


namespace ToyBox.Managers
{
    public class BuildsManager : MonoBehaviour
    {
        public static BuildsManager Instance { get; private set; }

        public PlayerManager playerManager;

        [SerializeField] List<PlaceableStruct> objectsStruct = new();

        public List<Build> objects = new();


        int picked = 0;
        int turnNumber = 0;
        List<Build> objectsList = new();

        public bool selecting = false;

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

        public void AddObject(Build build) {
            
            if (build.erase)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    Build b = objects[i];
                    bool toDelete = false;

                    foreach (var offset in b.offsets)
                    {
                        if (build.ContainsPos((Vector2)b.transform.position + offset))
                        {
                            toDelete = true;
                            break;
                        }
                    }

                    if(toDelete)
                    {
                        objects.RemoveAt(i);
                        Destroy(b.gameObject);
                        i--;
                    }
                }

                Destroy(build.gameObject);
            }
            else
            {
                build.Place(true);
                objects.Add(build);
            }

            if (playerManager.DoesAllPlayersFinishedBuilding())
            {
                print("1");
                GameModeManager.Instance.StartCountDown(3.5f);
            }

            print("2");
        }

        public void Shuffle(int amount)
        {
            if (turnNumber <= 10)
            {
                turnNumber++;
            }
            selecting = true;
            amount = playerManager.Players.Count + 3;

            ChooseBox.Instance.gameObject.SetActive(true);

            for (int i = 0; i < amount; i++)
            {
                float probability = 0;
                GameObject chosenObject = objectsStruct[0]._objectPrefab;
                for (int j = 0; j < objectsStruct.Count; j++)
                {

                    float objectProbability = Random.Range(0, objectsStruct.Count*.1f)*objectsStruct[j]._curve.Evaluate(turnNumber*.1f);
                    if (probability <= objectProbability)
                    {
                       
                        
                        probability = objectProbability;
                        chosenObject = objectsStruct[j]._objectPrefab;
                    }
                    Debug.Log($"Object : {objectsStruct[j]._objectPrefab.name} with probability : {objectProbability}");

                }
                

                
                GameObject go = Instantiate(chosenObject, new(Random.Range(ChooseBox.Instance.BL.position.x, ChooseBox.Instance.TR.position.x), Random.Range(ChooseBox.Instance.BL.position.y, ChooseBox.Instance.TR.position.y)), Quaternion.identity, ChooseBox.Instance.transform);
                Build b = go.GetComponent<Build>();
                
                objectsList.Add(b);
                b.pickedEvent.AddListener(ObjectPicked);
            }
        }

        public void ObjectPicked()
        {
            picked++;
            if (picked == playerManager.Players.Count)
            {
                ChooseBox.Instance.gameObject.SetActive(false);

                picked = 0;

                foreach (Build b in objectsList)
                {
                    if(!b.chosen) Destroy(b.gameObject);
                }

                objectsList.Clear();
                
                selecting = false;
            }
        }

        public bool CanPlace(Build testedBuild)
        {
            foreach (Build build in objects)
            {
                foreach (var offset in build.offsets)
                {
                    if (testedBuild.ContainsPos((Vector2)build.transform.position + offset))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
    [System.Serializable]
    public struct PlaceableStruct
    {
        public GameObject _objectPrefab;
        public AnimationCurve _curve;

    }
}