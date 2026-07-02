using System;
using UnityEngine;
[CreateAssetMenu]
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
