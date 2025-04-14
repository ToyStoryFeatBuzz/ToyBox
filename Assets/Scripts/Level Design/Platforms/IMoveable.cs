using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public interface IMoveable
{
    public Transform PosA { get; }
    public Transform PosB { get; }

    public void Movement();




}
