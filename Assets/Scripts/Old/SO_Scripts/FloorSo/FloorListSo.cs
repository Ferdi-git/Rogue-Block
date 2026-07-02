using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloorListSo : ScriptableObject
{
    public List<FloorEvent> list = new List<FloorEvent> ();
}
