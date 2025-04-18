using ToyBox.Player;
using UnityEngine;

public class speedUltimate : ulti
{
    [SerializeField] private float _speedBoost;
    private PlayerMovement _movement;
    
    private float _baseSpeed;
    
    void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _baseSpeed = _movement.MaxSpeed;
    }

    public override void Ultimate()
    {
        _movement.MaxSpeed += _speedBoost;
    }

    public override void RestoreDefaultState()
    {
        _movement.MaxSpeed = _baseSpeed;
    }
}
