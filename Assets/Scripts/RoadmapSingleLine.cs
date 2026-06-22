using System.Collections.Generic;
using UnityEngine;

public class RoadmapSingleLine : MonoBehaviour
{
    public int intLine= 1;
    public GameObject[] points;

    [HideInInspector] public List<bool> used = new List<bool>();
    private List<string> types = new List<string>();

    private void Awake()
    {
        InitLists();
    }

    private void InitLists()
    {
        used.Clear();
        types.Clear();
        for (int i = 0; i < points.Length; i++)
        {
            used.Add(false);
            types.Add("");
        }
    }

    public void ClearLine()
    {
        InitLists();

    }

    public void SetPointType(int i , Encouter encouter)
    {
        types[i] = encouter.name;
        points[i].GetComponent<SpriteRenderer>().sprite = encouter.image;
    }

    public string GetType(int i)
    {
        return types[i];
    }
}
