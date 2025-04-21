using System;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour,IMoveable
{

    [SerializeField] private Transform _posA;
    [SerializeField] private Transform _posB;
    public Transform PosA { get=>_posA; }
    public Transform PosB { get=>_posB; }
    
    [SerializeField] private bool _slerp;
    
    [SerializeField] private float _speed;

    private float _direction=1; 
    private float _timer=0;
    

    private Func<Vector3, Vector3, float,Vector3> _interpolationMovement;
    
    private void Start()
    {
        if (_slerp)
        {
            _interpolationMovement= ((A, B, T) => Vector3.Slerp(A, B, T));
        }
        else
        {
            _interpolationMovement= ((A, B, T) => Vector3.Lerp(A, B, T));
        }
    }

    public void Movement()
    {
        _timer += Time.deltaTime*_direction*_speed;
        transform.position=_interpolationMovement.Invoke(PosA.position, PosB.position, _timer);
        if (_timer>.90f)
        {
            _direction = -1;
        }
        else if(_timer<0.1f)
        {
            _direction = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }


}
