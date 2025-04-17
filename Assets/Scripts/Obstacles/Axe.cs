using UnityEngine;

public class AxeSwing : MonoBehaviour
{
    [SerializeField] private float swingAngle = 45f;
    [SerializeField] private float swingSpeed = 1f;

    private void Update()
    {
        float angle = Mathf.PingPong(Time.time * swingSpeed, swingAngle * 2) - swingAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
