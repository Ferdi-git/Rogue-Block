using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventTrail : ScriptableObject 
{
    public event Action<EventTrailData> CreateTrail;
    public void InvokeCreateTrail(EventTrailData newData) { CreateTrail?.Invoke(newData); }
   
}

public class EventTrailData
{
    public Vector3 pos1;
    public Vector3 pos2;
    public float height = 1;
    public float trailTime = 0.15f;
    public Color glowColor;
    public Action eventEndTrail;
}
