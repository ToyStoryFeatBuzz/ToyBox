using UnityEngine;

public class ManagerHolder : MonoBehaviour
{
    public static ManagerHolder Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
