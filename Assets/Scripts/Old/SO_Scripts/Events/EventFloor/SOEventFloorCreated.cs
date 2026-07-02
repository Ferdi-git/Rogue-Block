using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventFloorCreated : ScriptableObject
{
    public event Action FloorCreated;

    public void InvokeFloorCreated()
    {
        FloorCreated?.Invoke(); 
    }
}
