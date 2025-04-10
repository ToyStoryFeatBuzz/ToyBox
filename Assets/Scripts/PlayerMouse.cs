using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    Vector2 mousePos = Vector2.zero;

    [SerializeField] Transform mouseBody;
    [SerializeField] Transform mouseBodyPrefab;

    [SerializeField] float mouseSensivity;

    private void Awake()
    {
        if(!mouseBody)
            mouseBody = Instantiate(mouseBodyPrefab);
        ResetMousePos();
        mouseBody.position = mousePos;
        ActivateMouse(false);
    }

    private void ResetMousePos()
    {
        mousePos = Camera.main.transform.position;
    }

    public void Move(Vector2 movement)
    {
        mousePos += movement * mouseSensivity;
        mouseBody.position = mousePos;
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
