using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/SOEventVisualNumber")]
public class SOEventVisualNumber : ScriptableObject 
{
    public event Action<EventVisualNbrData> CreateVisualNumber;
    public void InvokeCreateVisualNumber(EventVisualNbrData newData) { CreateVisualNumber?.Invoke(newData); }
   
}

public class EventVisualNbrData
{
    public Vector3 spawnPoint;
    public int nbr;
    public Color color;
    public bool isPositive = true;


}
