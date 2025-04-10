using System.Collections.Generic;
using ToyBox.Managers;
using Toybox.InputSystem;
using UnityEngine;

public class PlayerEdition : MonoBehaviour
{
    GameObject draggedObject = null;

    [SerializeField] bool doesSnap;
    [SerializeField] float snapInterval;

    Vector2 mousePos;

    [SerializeField] List<GameObject> objectsPrefabs = new();

    Vector2 lastDifferentPos = Vector2.zero;
    bool placeable = false;

    BuildsManager buildsManager;

    PlayerMouse playerMouse;

    PlayerInputSystem playerInputManager;

    private void Start()
    {
        buildsManager = BuildsManager.Instance;
        playerMouse = GetComponent<PlayerMouse>();
        playerInputManager = GetComponent<PlayerInputSystem>();

        playerInputManager.OnPlaceEvent.Canceled += Place;
        playerInputManager.OnRotateRightEvent.Performed += ()=>{ Rotate(90); };
        playerInputManager.OnRotateLeftEvent.Performed += ()=>{ Rotate(-90); };
    }

    private void OnEnable()
    {
        //SetRandomObject();
        if(!playerMouse) playerMouse = GetComponent<PlayerMouse>();
        playerMouse.ActivateMouse(true);
    }

    private void OnDisable()
    {
        if(draggedObject != null) Destroy(draggedObject);
        playerMouse.ActivateMouse(false);
    }

    public void SetRandomObject()
    {
        SelectObject(objectsPrefabs[Random.Range(0, objectsPrefabs.Count)]);
    }

    public void SelectObject(GameObject go)
    {
        if(draggedObject != null) Destroy(draggedObject);

        draggedObject = Instantiate(go);
    }

    public void Place()
    {
        if (draggedObject)
        {
            if (buildsManager.selecting) return;

            placeable = buildsManager.CanPlace(draggedObject.GetComponent<Build>());

            if (!placeable) return;

            placeable = false;

            buildsManager.AddObject(draggedObject.GetComponent<Build>());

            draggedObject = null;
            enabled = false;
            //SetRandomObject();
        }
        else
        {
            Collider2D hit = Physics2D.OverlapCircle(mousePos, .1f);
            if (hit && hit.transform && hit.transform.GetComponentInParent<Build>())
            {
                Build obj = hit.transform.GetComponentInParent<Build>();

                if (obj.chosen) return;

                obj.Pick();
                draggedObject = obj.gameObject;
                placeable = buildsManager.CanPlace(draggedObject.GetComponent<Build>());
            }
        }
    }

    public void Rotate(float angle)
    {
        if (!draggedObject) return;
        draggedObject.transform.eulerAngles = new(0, 0, draggedObject.transform.eulerAngles.z + angle);
        draggedObject.GetComponent<Build>().RotateOffsets(angle);
        placeable = buildsManager.CanPlace(draggedObject.GetComponent<Build>());
    }

    private void Update()
    {
        if (playerInputManager.GridMoveDir.magnitude > 0) playerMouse.Move(playerInputManager.GridMoveDir);

        mousePos = playerMouse.Click();

        if (draggedObject == null) return;

        Vector2 targetPos = mousePos + (Vector2.one * snapInterval / 2f);

        if (doesSnap)
        {
            int x = Mathf.FloorToInt(targetPos.x / snapInterval);
            int y = Mathf.FloorToInt(targetPos.y / snapInterval);
            targetPos.Set(x * snapInterval, y * snapInterval);
        }

        draggedObject.transform.position = targetPos;

        if (lastDifferentPos != targetPos)
        {
            placeable = buildsManager.CanPlace(draggedObject.GetComponent<Build>());
            lastDifferentPos = targetPos;
        }
    }
}
