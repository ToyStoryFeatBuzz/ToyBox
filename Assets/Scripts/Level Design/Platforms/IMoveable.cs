using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public interface IMoveable
{
    public Transform posA { get; }
    public Transform posB { get; }

    public void Movement();




}
