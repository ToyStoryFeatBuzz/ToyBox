using System;
using UnityEngine;

public class ManagerHolder : MonoBehaviour
{
    private static ManagerHolder Instance;

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
