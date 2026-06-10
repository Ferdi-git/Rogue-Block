using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ManaManager : MonoBehaviour
{

    [SerializeField] SOEventVisuelEffect visuelEffect;
    [SerializeField] SOEventTrail eventTrail;
    [SerializeField] Transform posMana;
    [SerializeField, ColorUsage(true, true)] Color glowManaColor;
    [SerializeField] StatsPlayer statsPlayer;

    private void OnEnable()
    {
        visuelEffect.EffectGainMana += PlayerGetMana;
    }

    private void OnDisable()
    {
        visuelEffect.EffectGainMana -= PlayerGetMana;

    }


    private void PlayerGetMana(VisuelAttakData data)
    {
        StartCoroutine(GetMana(data));
    }
    private IEnumerator GetMana(VisuelAttakData data)
    {
        bool ended = false;
        Action trailEvent = () => ended = true;

        eventTrail.InvokeCreateTrail(new EventTrailData()
        {
            pos1 = data.posAttacker,
            pos2 = posMana.position,
            height = 1,
            trailTime = 0.15f,
            glowColor = glowManaColor,
            eventEndTrail = trailEvent,
        });

        statsPlayer.InvokeGainMana(data.nbrDMG%5);

        for (int i = 0;  i < Mathf.Round(data.nbrDMG/5) ; i++)
        {
            Action nulltrailEvent = () => ended = true;

            yield return new WaitForSeconds(0.05f);
            statsPlayer.InvokeGainMana(5);
            eventTrail.InvokeCreateTrail(new EventTrailData()
            {
                pos1 = data.posAttacker,
                pos2 = posMana.position,
                height = UnityEngine.Random.Range(0.1f,1f) ,
                trailTime = UnityEngine.Random.Range(0.07f, 0.14f),
                glowColor = glowManaColor,
                eventEndTrail=nulltrailEvent,
            });
            
        }
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => ended);

        data.eventEndVisuel.Invoke();
    }


}
