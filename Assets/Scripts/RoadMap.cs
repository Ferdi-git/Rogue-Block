using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class RoadMap : MonoBehaviour
{
    [SerializeField] GameObject firstPoint;
    [SerializeField] RoadmapSingleLine[] lines;
    [SerializeField] LineRenderer[] lineRenderers;

    [Button]
    void GenerateRoadmap()
    {
        ClearRoad();

        for (int i = 0; i < lineRenderers.Length; i++)
        {

            lineRenderers[i].positionCount = lines.Length + 1;
            lineRenderers[i].SetPosition(0, firstPoint.transform.position);
            int nextRand = Random.Range(1, 3);

            for (int l = 0; l < lines.Length; l++)
            {
                lineRenderers[i].SetPosition(l + 1, lines[l].points[nextRand].transform.position);
                lines[l].used[nextRand] = true;
                int randInt = Random.Range(-1, 2);
                if (l + 1 < lines.Length)
                {
                    int newRandInt = Random.Range(-1, 2);

                    if (newRandInt + nextRand > lines[l + 1].points.Length - 1 || newRandInt + nextRand < 0)
                        newRandInt = -(newRandInt);

                    if (randInt == 0 && !lines[l].used[nextRand+ newRandInt])
                        randInt = newRandInt;



                    if (randInt + nextRand > lines[l + 1].points.Length - 1 || randInt + nextRand < 0)
                        randInt = 0;

                    if (randInt != 0 && lines[l + 1].used[nextRand])
                        randInt = 0;

                    nextRand = randInt + nextRand;

                }

            }


        }

        HidePoints();

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
