using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyVisuelManager : MonoBehaviour
{

    [SerializeField] SOEventVisuelEffect visuelEffect;
    [SerializeField] SOEventTrail eventTrail;
    [SerializeField] SOEventVisualNumber eventNumbers;
    [SerializeField] SOEventEnnemy statsEnnemi;
    [SerializeField] Transform posEnemy;
    [SerializeField, ColorUsage(true, true)] Color glowAtkColor;
    [SerializeField] Animator enemyAnimator; 
    [SerializeField] Animator playerAnimator;
    [SerializeField] SOEventPlayer eventPlayer;

    private void OnEnable()
    {
        visuelEffect.EffectAtkEnemy += EnemyGetAttacked;
        visuelEffect.EffectEnemyDealAtk += EnemyAttack;

        statsEnnemi.EnnemiLoosePV += EnnemiLoosePV;
        statsEnnemi.EnnemiGainPV += EnnemiGainPV;
        eventPlayer.LoosePV += PlayerLoosePV;
    }

    private void OnDisable()
    {
        visuelEffect.EffectAtkEnemy -= EnemyGetAttacked;
        visuelEffect.EffectEnemyDealAtk -= EnemyAttack;

        statsEnnemi.EnnemiLoosePV -= EnnemiLoosePV;
        statsEnnemi.EnnemiGainPV -= EnnemiGainPV;
        eventPlayer.LoosePV -= PlayerLoosePV;


    }


    private void EnemyGetAttacked(VisuelAttakData data)
    {
        StartCoroutine(GetAttackCo(data));
    }
    private IEnumerator GetAttackCo(VisuelAttakData data)
    {
        bool ended = false;
        Action trailEvent = () => ended = true;
        eventTrail.InvokeCreateTrail(new EventTrailData()
        {
            pos1 = data.posAttacker,
            pos2 = posEnemy.position,
            height = 1,
            trailTime = 0.15f - 0.005f ,
            glowColor = glowAtkColor,
            eventEndTrail = trailEvent,
        });
        yield return new WaitUntil(() => ended);
        enemyAnimator.SetTrigger("TakeDMG");

        data.eventEndVisuel.Invoke();
    }


    private void EnemyAttack(VisuelAttakData data)
    {
        StartCoroutine(AttackCo(data));
    }
    private IEnumerator AttackCo(VisuelAttakData data)
    {
        bool ended = false;
        Action trailEvent = () => ended = true;
        eventTrail.InvokeCreateTrail(new EventTrailData()
        {
            pos1 = posEnemy.position,
            pos2 = data.posAttacker,
            height = 1,
            trailTime = 0.15f - 0.005f,
            glowColor = glowAtkColor,
            eventEndTrail = trailEvent,
        });
        yield return new WaitUntil(() => ended);

        data.eventEndVisuel.Invoke();
    }

    private void EnnemiLoosePV(int nbr)
    {
        EventVisualNbrData newData = new EventVisualNbrData(); 
        newData.nbr = nbr;
        newData.spawnPoint = posEnemy.position;
        newData.color = Color.red;
        eventNumbers.InvokeCreateVisualNumber(newData);
    }


    private void EnnemiGainPV(int nbr)
    {
        EventVisualNbrData newData = new EventVisualNbrData();
        newData.nbr = nbr;
        newData.spawnPoint = posEnemy.position;
        newData.color = Color.green;
        eventNumbers.InvokeCreateVisualNumber(newData);

    }

    private void PlayerLoosePV(int pv)
    {
        playerAnimator.SetTrigger("TakeDMG");

    }
}
