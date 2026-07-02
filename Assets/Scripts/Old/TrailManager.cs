using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrailManager : MonoBehaviour
{
    [SerializeField] GameObject SingleTrailPrefab;
    [SerializeField] SOEventTrail SOEventTrail;

    private void OnEnable()
    {
        SOEventTrail.CreateTrail += CreatePara;
    }

    private void OnDisable()
    {
        SOEventTrail.CreateTrail -= CreatePara;

    }

    private void CreatePara(EventTrailData p)
    {
        StartCoroutine(CreateParaBole(p));
    }
    public IEnumerator CreateParaBole(EventTrailData trailData)
    {
        GameObject newTrail = Instantiate(SingleTrailPrefab);
        TrailRenderer trailRenderer = newTrail.GetComponent<TrailRenderer>();

        Material mat = new Material(trailRenderer.material);
        trailRenderer.material = mat;


        mat.SetColor("_GlowColor", trailData.glowColor);
        newTrail.transform.position = trailData.pos1;
        trailRenderer.Clear();

        float T = 0;
        float timeSinceStart = 0;
        while (timeSinceStart < trailData.trailTime)
        {
            timeSinceStart += Time.deltaTime;
            T = timeSinceStart / trailData.trailTime;

            newTrail.transform.position = SampleParabola(trailData.pos1, trailData.pos2, trailData.height, T, Vector3.up);
            yield return new WaitForEndOfFrame();
        }
        Destroy(newTrail);
        trailData.eventEndTrail.Invoke();
    }

    private Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection)
    {
        float parabolicT = t * 2 - 1;

        Vector3 travelDirection = end - start;
        Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
        Vector3 right = Vector3.Cross(travelDirection, levelDirection);
        Vector3 up = outDirection;
        Vector3 result = start + t * travelDirection;
        result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
        return result;
    }
}
