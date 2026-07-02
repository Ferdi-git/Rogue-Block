using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Old/SOEventFloor")]
public class SOEventFloor : ScriptableObject
{
    public event Action FirstFloorGeneration;
    public event Action NextFloor;

    public void InvokeFirstFloor()
    {
        FirstFloorGeneration?.Invoke();
    }

    public void InvokeNextFloor()
    {
        NextFloor?.Invoke();
    }
}
