using UnityEngine;

public interface IMoveable
{
    public Transform PosA { get; }
    public Transform PosB { get; }

    public void Movement();




}
