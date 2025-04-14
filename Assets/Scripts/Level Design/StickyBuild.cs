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
            _buildsManager.ObjectPlaced.AddListener(CheckAroundForGluedItems);
        }

        void CheckAroundForGluedItems()
        {
            Debug.Log("Checking Around For Glued Items");
            _originalPosition = transform.position;
            var globalGluePoints = GluePoints.Select(gp => _originalPosition + gp).ToHashSet();

            foreach (var block in _buildsManager.Objects.Where(b => !_gluedObjects.Contains(b)))
            {
                Vector2 blockPos = (Vector2)block.transform.position;

                if (block.Offsets.Any(offset => globalGluePoints.Contains(blockPos + offset)))
                {
                    Debug.Log("Sticky");
                    _gluedObjects.Add(block);
                    _movingBlock.transform.position = _originalPosition;
                    block.gameObject.transform.parent = _movingBlock.transform;
                }
            }

        }
        
        //void CheckAroundForGluedItems()
        //{
        //    _originalPosition = transform.position;
        //    Debug.Log("Checking for glued objects");
        //    foreach (BuildObject block in _buildsManager.Objects)
        //    {
        //        foreach (Vector2 offset in block.Offsets)
        //        {
        //            foreach (Vector2 glueSpot in GluePoints)
        //            {
        //                if ((Vector2)block.transform.position + offset == _originalPosition+glueSpot && !_gluedObjects.Contains(block)) //Checks if object is in range of any glue spot AND is not already glued to the block
        //                {
        //                    Debug.Log("Sticky");
        //                    _gluedObjects.Add(block);
        //                    _movingBlock.transform.position = _originalPosition;
        //                    block.gameObject.transform.parent = _movingBlock.transform; //Change the glued block's parent to be the sticky block
        //                }
        //            }
        //        }
        //    }
        //}

    }
}
