using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    Vector2 mousePos = Vector2.zero;

    [SerializeField] Transform mouseBody;
    [SerializeField] Transform mouseBodyPrefab;

    [SerializeField] float mouseSensivity;

    float maxX;
    float maxY;

    Camera cam;

    private void Awake()
    {
        if(!mouseBody)
            mouseBody = Instantiate(mouseBodyPrefab);
        ResetMousePos();
        mouseBody.localPosition = mousePos;
        ActivateMouse(false);
        cam = Camera.main;
        mouseBody.parent = cam.transform;

    }


    private void SetMaxPos()
    {
        if(!cam) cam = Camera.main;

        maxX = cam.orthographicSize * cam.aspect;
        maxY = cam.orthographicSize;
    }

    private void ResetMousePos()
    {
        mousePos = Vector2.zero;
    }

    public void Move(Vector2 movement)
    {
        SetMaxPos();

        Vector2 camPos = cam.transform.position;
        mousePos += movement * mouseSensivity;
        mousePos.Set(Mathf.Clamp(mousePos.x, -maxX, maxX), Mathf.Clamp(mousePos.y, -maxY, maxY));
        mouseBody.position = camPos + mousePos;
    }

    public Vector2 Click()
    {
        return mousePos;
    }

    public void ActivateMouse(bool activation)
    {
        if (!mouseBody)
        {
            mouseBody = Instantiate(mouseBodyPrefab);
            ResetMousePos();
            mouseBody.position = mousePos;
        }
        mouseBody?.gameObject?.SetActive(activation);
        Cursor.visible = !activation;
    }
}
