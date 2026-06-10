
using UnityEngine;
using UnityEngine.UI;

public class EnnemiManager : MonoBehaviour
{
    [SerializeField] private SOEventEnnemy ennemiEvent;
    [SerializeField] private StatsEnnemi stats;
    [SerializeField] private GameObject ennemiUI;
    [SerializeField] private SpriteRenderer ennemiBackground;
    [SerializeField] private KeepEnnemiSo ennemiList;
    [SerializeField] private SOEventGridManager soEventGridManager;
    [SerializeField] private SOEventGiveUICurrentAtk UIeventGiveAtk;
    [SerializeField] private SOEventUpdateUI UIeventUpdateUI;
    private int index;
    private int atkIndex;
    private int CombatCount;

    private void OnEnable()
    {
        ennemiEvent.GenerateEnnemi += GenerateEnnemi;
        ennemiEvent.GenerateBoss += GenerateBoss;
        ennemiEvent.EnnemiShowAttack += ShowAtk;
        ennemiEvent.EnnemiRemoveAttack += RemoveAtk;
    }
    private void OnDisable()
    {
        ennemiEvent.GenerateEnnemi -= GenerateEnnemi;
        ennemiEvent.GenerateBoss -= GenerateBoss;
        ennemiEvent.EnnemiShowAttack -= ShowAtk;
        ennemiEvent.EnnemiRemoveAttack -= RemoveAtk;
    }


    public void GenerateEnnemi (int NbrOfCombat)
    {
        CombatCount = NbrOfCombat;
        ennemiUI.SetActive(true);
        atkIndex = 0;
        index = Random.Range(0, ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].ennemiList.Count);
        int backgroundIndex = Random.Range(0, ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].Backgrounds.Count);
        ennemiBackground.sprite = ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].Backgrounds[backgroundIndex];
        //stats.pvMax = ((ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].ennemiList[index].resistance/100) * stats.AverageValue) * (int)(1.2 * (NbrOfCombat+1)) ;
        stats.pvMax = ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].ennemiList[index].resistance;
        stats.pv = stats.pvMax;
        stats.shield = 0;
        stats.ennemiName = ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].ennemiList [index].Name;
        stats.sprite = ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].ennemiList[index].sprite;
        stats.ennemiAttacks = ennemiList.paliers[GiveCurrentPalier(NbrOfCombat)].ennemiList[index].attacks ;
        UIeventGiveAtk.InvokeGiveUICurrentAtk(0);
        UIeventUpdateUI.InvokeUpdateUI();
        
        
    }


    public void GenerateBoss (int BossIndex)
    {
        if (BossIndex >= ennemiList.bossList.Count)
        {
            BossIndex = ennemiList.bossList.Count - 1;
        }
        ennemiUI.SetActive(true);
        atkIndex = 0;
        stats.pvMax = ennemiList.bossList[BossIndex].resistance;
        stats.pv = stats.pvMax;
        stats.shield = 0;
        stats.ennemiName = ennemiList.bossList[BossIndex].Name;
        stats.sprite = ennemiList.bossList[BossIndex].sprite;
        stats.ennemiAttacks = ennemiList.bossList[BossIndex].attacks;
        UIeventGiveAtk.InvokeGiveUICurrentAtk(0);
        UIeventUpdateUI.InvokeUpdateUI();
    }
    private int GiveCurrentPalier (int NbrOfCombat)
    {
        if (CombatCount == 0)
        {
            return 0 ;
        }
        else if (CombatCount <= 4 )
        {
            return 1; 
        }
        else if (CombatCount <= 9 )
        {
            return 2;
        }
        else if (CombatCount <= 14)
        {
            return 3;
        }
        else
        {
            return 3;
        }


    }

    private void ShowAtk ()
    {
        atkIndex = Random.Range (0, stats.ennemiAttacks.Count);
        UIeventGiveAtk.InvokeGiveUICurrentAtk(atkIndex);
        soEventGridManager.InvokeSelectRandomSlot(stats.ennemiAttacks[atkIndex].zone);
        stats.actualAtkDamage = GetDamageValue();
        stats.actualAtkZoneNbr = GetAtkZoneNbr();
    }
    private void RemoveAtk ()
    {
        soEventGridManager.InvokeRemoveAtk();
    }
    
    private int GetDamageValue ()
    {
        float damage = stats.ennemiAttacks[atkIndex].damage ;
        //+CombatCount
        return (int) damage;
    }

    private int GetAtkZoneNbr ()
    {
        return stats.ennemiAttacks[atkIndex].zone.gameObject.GetComponent<EnemyZoneAtk>().listSquareAtk.Count;
    }

}
