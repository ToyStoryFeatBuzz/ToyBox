using System.Collections.Generic;
using Managers;
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

    private void Start()
    {
        buildsManager = BuildsManager.Instance;
    }

    private void OnEnable()
    {
        SetRandomObject();
    }

    private void OnDisable()
    {
        if(draggedObject != null) Destroy(draggedObject);
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
        if (!placeable) return;

        placeable = false;

        buildsManager.AddObject(draggedObject.GetComponent<Build>());

        draggedObject = null;
        SetRandomObject();
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
        if (Input.GetKeyDown(KeyCode.H)) { Rotate(90f); }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // To change

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
