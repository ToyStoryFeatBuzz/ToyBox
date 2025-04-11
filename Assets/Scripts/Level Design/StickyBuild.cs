using System.Collections.Generic;
using System.Linq;
using ToyBox.Managers;
using UnityEngine;

[RequireComponent(typeof(Build))]
public class StickyBuild : MonoBehaviour {
    
    public List<Vector2> GluePoints = new();

    private List<Build> gluedObjects = new();
    
    BuildsManager _buildsManager => BuildsManager.Instance;

    void Start()
    {
        _buildsManager.ObjectPlaced.AddListener(CheckAroundForGluedItems);
    }

    void CheckAroundForGluedItems()
    {
        Debug.Log("Checking for glued objects");
        foreach (Build block in _buildsManager.objects)
        {
            foreach (Vector2 offset in block.offsets)
            {
                foreach (Vector2 glueSpot in GluePoints)
                {
                    if ((Vector2)block.transform.position + offset == (Vector2)transform.position+glueSpot && !gluedObjects.Contains(block)) //Checks if object is in range of any glue spot AND is not already glued to the block
                    {
                        Debug.Log("Sticky");
                        gluedObjects.Add(block);
                        block.gameObject.transform.parent = gameObject.transform; //Change the glued block's parent to be the sticky block
                    }
                }
            }
        }
    }
    
}
