using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class RoadMap : MonoBehaviour
{
    [SerializeField] GameObject firstPoint;
    [SerializeField] RoadmapSingleLine[] lines;
    [SerializeField] LineRenderer[] lineRenderers;
    [SerializeField] Encouter[] encouters; 

    [Button]
    void GenerateRoadmap()
    {
        ClearRoad();

        for (int i = 0; i < lineRenderers.Length; i++)
        {
            SetupLineRenderer(lineRenderers[i], lines, firstPoint);
        }

        HidePoints();

    }

    private void SetupLineRenderer(LineRenderer renderer, RoadmapSingleLine[] lines, GameObject firstPoint)
    {
        renderer.positionCount = lines.Length + 1;
        renderer.SetPosition(0, firstPoint.transform.position);

        int pointIndex = UnityEngine.Random.Range(1, 3); 

        for (int l = 0; l < lines.Length; l++)
        {
            renderer.SetPosition(l + 1, lines[l].points[pointIndex].transform.position);
            lines[l].used[pointIndex] = true;

            int randEncounter = UnityEngine.Random.Range(0, encouters.Length);
            lines[l].SetPointType(pointIndex,encouters[randEncounter]);

            int randomDir = UnityEngine.Random.Range(-1, 2); // -1, 0 or 1

            if (l + 1 >= lines.Length)
                continue; 

            int nextColumnSize = lines[l + 1].points.Length;
            int altRandomDir = UnityEngine.Random.Range(-1, 2);

            if (!IsValidIndex(pointIndex + altRandomDir, nextColumnSize))
                altRandomDir = -altRandomDir;

            if (randomDir == 0 && !lines[l].used[pointIndex + altRandomDir])
                randomDir = altRandomDir;

            if (!IsValidIndex(pointIndex + randomDir, nextColumnSize))
                randomDir = 0;

            if (randomDir != 0 && lines[l + 1].used[pointIndex])
                randomDir = 0;

            pointIndex += randomDir;



        }
    }
    private bool IsValidIndex(int index, int length)
    {
        return index >= 0 && index < length;
    }

    void ClearRoad()
    {
        for (int i = 0;i < lineRenderers.Length; i++)
        {
            lineRenderers[i].positionCount = 0;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].ClearLine();
        }

        for(int i = 0; i< lines.Length; i++)
        {
            for(int p = 0;  p < lines[i].used.Count; p++)
            {
                lines[i].used[p] = false;
                lines[i].points[p].SetActive(true);

            }

        }

    }

    void HidePoints()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            for (int p = 0; p < lines[i].used.Count; p++)
            {
                if(lines[i].used[p] == false)
                {
                    lines[i].points[p].SetActive(false);
                }
            }

        }
    }



}
[Serializable]
public class Encouter
{
    public string name;
    public Sprite image;
    public float weight;
}