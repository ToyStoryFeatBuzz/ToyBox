using System.Collections.Generic;
using UnityEngine;


namespace ToyBox.Managers
{
    public class BuildsManager : MonoBehaviour
    {
        public static BuildsManager Instance { get; private set; }

        public GameObject btndemerde;

        public PlayerManager playerManager;

        [SerializeField] List<GameObject> objectsPrefabs = new();

        public List<Build> objects = new();

        [Header("ChooseBox")]
        [SerializeField] GameObject objectsBox;
        [SerializeField] Transform topRight;
        [SerializeField] Transform bottomLeft;

        int picked = 0;
        List<Build> objectsList = new();

        public bool selecting = false;

        private void Awake()
        {
            objectsBox.SetActive(false);
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

        public void AddObject(Build build)
        {
            

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

                return;
            }
            build.Place(true);
            objects.Add(build);
        }

        public void Shuffle(int amount)
        {
            selecting = true;
            btndemerde.SetActive(false);
            amount = playerManager.Players.Count + 3;

            objectsBox.SetActive(true);

            for (int i = 0; i < amount; i++)
            {
                GameObject go = Instantiate(objectsPrefabs[Random.Range(0, objectsPrefabs.Count)], new(Random.Range(bottomLeft.position.x, topRight.position.x), Random.Range(bottomLeft.position.y, topRight.position.y)), Quaternion.identity);
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
                objectsBox.SetActive(false);

                picked = 0;

                foreach (Build b in objectsList)
                {
                    if(!b.chosen) Destroy(b.gameObject);
                }

                objectsList.Clear();

                btndemerde.SetActive(true);
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
}