using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/FloorListSo")]
public class FloorListSo : ScriptableObject
{
    public List<FloorEvent> list = new List<FloorEvent> ();
}
