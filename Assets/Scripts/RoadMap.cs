using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadMap : MonoBehaviour
{
    [SerializeField] GameObject firstPoint;
    [SerializeField] RoadmapSingleLine[] lines;
    [SerializeField] LineRenderer[] lineRenderers;
    private List<int> firstRenderIndex;
    private List<int> branchIndex;
    [SerializeField] Encouter[] encouters;
    [SerializeField] SOEventFloorCreated floorCreated;

    [Button]
    void GenerateRoadmap()
    {
        ClearRoad();

        branchIndex = new List<int>();

        SetupLineRenderer(lineRenderers[0], lines, firstPoint);

        for (int i = 1; i < lineRenderers.Length; i++)
        {
            //SetupLineRenderer(lineRenderers[i], lines, firstPoint);
            SetupJunction(i);
        }


        HidePoints();

    }

    private void SetupLineRenderer(LineRenderer renderer, RoadmapSingleLine[] lines, GameObject firstPoint)
    {
        firstRenderIndex = new List<int>();
        firstRenderIndex.Clear();

        renderer.positionCount = lines.Length + 1;
        renderer.SetPosition(0, firstPoint.transform.position);

        int pointIndex = UnityEngine.Random.Range(1, 3);

        for (int l = 0; l < lines.Length; l++)
        {
            firstRenderIndex.Add(pointIndex);
            renderer.SetPosition(l + 1, lines[l].points[pointIndex].transform.position);
            lines[l].used[pointIndex] = true;

            int randEncounter = UnityEngine.Random.Range(0, encouters.Length);
            lines[l].SetPointType(pointIndex, encouters[randEncounter]);

            if (l + 1 >= lines.Length)
                continue;

            pointIndex = GetNextPointIndex(pointIndex, l);
        }
    }


    private void SetupJunction(int iLineRenderer)
    {

        LineRenderer renderer = lineRenderers[iLineRenderer];

        int randomStartPoint = UnityEngine.Random.Range(1, lineRenderers[0].positionCount - 1);

    
        int pointIndex = GetNextPointIndex(firstRenderIndex[randomStartPoint - 1], randomStartPoint - 1);

        int nbrTry = 0;
        while ((pointIndex == firstRenderIndex[randomStartPoint] && nbrTry < 100) || branchIndex.Contains(randomStartPoint))
        {
            nbrTry++;
            randomStartPoint = UnityEngine.Random.Range(1, lineRenderers[0].positionCount - 1);
            pointIndex = GetNextPointIndex(firstRenderIndex[randomStartPoint - 1], randomStartPoint - 1);
        }

        branchIndex.Add(randomStartPoint);
        renderer.positionCount = lines.Length + 1 - randomStartPoint;
        renderer.SetPosition(0, lineRenderers[0].GetPosition(randomStartPoint));


        for (int l = randomStartPoint; l < lines.Length; l++)
        {
            renderer.SetPosition(l + 1 - randomStartPoint, lines[l].points[pointIndex].transform.position);
            lines[l].used[pointIndex] = true;

            int randEncounter = UnityEngine.Random.Range(0, encouters.Length);
            lines[l].SetPointType(pointIndex, encouters[randEncounter]);

            if (l + 1 >= lines.Length)
                continue;

            pointIndex = GetNextPointIndex(pointIndex, l);
        }
    }


    private int GetNextPointIndex(int currentIndex, int currentLine)
    {
        int nextColumnSize = lines[currentLine + 1].points.Length;

        int randomDir = UnityEngine.Random.Range(-1, 2); // -1, 0 or 1
        int altRandomDir = UnityEngine.Random.Range(-1, 2);

        if (!IsValidIndex(currentIndex + altRandomDir, nextColumnSize))
            altRandomDir = -altRandomDir;

        if (randomDir == 0 && !lines[currentLine].used[currentIndex + altRandomDir])
            randomDir = altRandomDir;

        if (!IsValidIndex(currentIndex + randomDir, nextColumnSize))
            randomDir = 0;

        if (randomDir != 0 && lines[currentLine + 1].used[currentIndex])
            randomDir = 0;

        return currentIndex + randomDir;
    }

    private bool IsValidIndex(int index, int length)
    {
        return index >= 0 && index < length;
    }

    void ClearRoad()
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].positionCount = 0;
        }

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].ClearLine();
        }

        for (int i = 0; i < lines.Length; i++)
        {
            for (int p = 0; p < lines[i].used.Count; p++)
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
                if (lines[i].used[p] == false)
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