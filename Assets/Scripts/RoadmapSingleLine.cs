using System.Collections.Generic;
using UnityEngine;

public class RoadmapSingleLine : MonoBehaviour
{
    public int intLine= 1;
    public GameObject[] points;

    [HideInInspector] public List<bool> used;

    private void Start()
    {
        for (int i = 0; i < points.Length; i++) used.Add(false);
    }

    public void ClearLine()
    {
        used.Clear();
        for (int i = 0; i < points.Length; i++) used.Add(false);

    }
}
