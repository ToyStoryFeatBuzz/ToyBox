using System.Collections.Generic;
using System.Linq;
using ToyBox.Managers;
using UnityEngine;

namespace ToyBox.Build
{
    [RequireComponent(typeof(BuildObject))]
    public class StickyBuild : MonoBehaviour
    {

        public List<Vector2> GluePoints = new();

        private List<BuildObject> _gluedObjects = new();

        Vector2 _originalPosition;

        [SerializeField] GameObject _movingBlock;

        BuildsManager _buildsManager;

        void Start()
        {
            _buildsManager = BuildsManager.Instance;
            _buildsManager.OnObjectPlaced += CheckAroundForGluedItems;
        }

        void CheckAroundForGluedItems()
        {
            _originalPosition = transform.position;
            HashSet<Vector2> globalGluePoints = GluePoints.Select(gp => _originalPosition + gp).ToHashSet();

            foreach (BuildObject block in _buildsManager.Objects.Where(b => !_gluedObjects.Contains(b)))
            {
                Vector2 blockPos = block.transform.position;

                if (!block.Offsets.Any(offset => globalGluePoints.Contains(blockPos + offset))) {
                    continue;
                }
                _gluedObjects.Add(block);
                _movingBlock.transform.position = _originalPosition;
                block.gameObject.transform.parent = _movingBlock.transform;
            }

        }
    }
}
