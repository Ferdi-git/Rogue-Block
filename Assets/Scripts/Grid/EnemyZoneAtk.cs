using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyZoneAtk : MonoBehaviour
{
    public List<SingleSquareAtk> listSquareAtk;
    [SerializeField] StatsEnnemi statsEnnemi;
    private List<GridSlot> listSlots;

    private void Start()
    {
        for (int i = 0; i < listSquareAtk.Count; i++) { listSquareAtk[i].SetText(statsEnnemi.actualAtkDamage.ToString());}
    }

    public bool CheckIfCanBePlaced()
    {
        listSlots = new List<GridSlot>();
        for (int i = 0; i < listSquareAtk.Count; i++)
        {
            bool foundSlot = false;

            foreach (var hit in Physics2D.OverlapPointAll(listSquareAtk[i].GetTransform().position))
            {
                GridSlot slot = hit.GetComponent<GridSlot>();

                if(slot != null && slot.text != null )
                {
                    listSlots.Add(slot);
                    foundSlot = true;
                }
            }

            if(!foundSlot)
            {
                return false;
            }

        }

        return true;
    }


    public void SetAtk()
    {
        for(int i = 0; i < listSlots.Count; i++)
        {
            listSlots[i].GetSelected();
        }

        for (int i = 0; i < listSquareAtk.Count; i++)
        {
            listSquareAtk[i].TurnTextRightSide();
        }

    }

    public void RemoveAtk()
    {
        for (int i = 0; i < listSlots.Count; i++)
        {
            listSlots[i].isAttacked = false;
        }
    }

}
