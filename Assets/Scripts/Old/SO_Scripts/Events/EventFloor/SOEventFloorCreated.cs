using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/SOEventFloorCreated")]
public class SOEventFloorCreated : ScriptableObject
{
    public event Action FloorCreated;

    public void InvokeFloorCreated()
    {
        FloorCreated?.Invoke(); 
    }
}
