using System.Collections.Generic;
using UnityEngine;


namespace Managers
{
    public class BuildsManager : MonoBehaviour
    {
        public static BuildsManager Instance { get; private set; }

        public List<Build> objects = new();

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

        public void AddObject(Build build)
        {
            objects.Add(build);
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