using System;
using TMPro;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    [SerializeField] ChoiceManager choiceManager;
    [SerializeField] StatsPlayer statsPlayer;
    [SerializeField] SOEventState state;
    [SerializeField] SOShop SOShop;

    [SerializeField] private TextMeshPro RerollText;
    [SerializeField] private TextMeshPro SkipText;

    private void Start()
    {
        SOShop.currentGainPV = SOShop.startGainPV;
        SOShop.currentLoosePV = SOShop.startLoosePV;
    }

    public void ResetShop()
    {
        choiceManager.GeneratePiece();
        statsPlayer.InvokeLoosePV(SOShop.currentLoosePV);
        SOShop.currentLoosePV += 1 ;
        RerollText.text = "-" + SOShop.currentLoosePV + "  <sprite name=heal 1>";
        if (statsPlayer.pv <= 0)state.InvokeLooseEvent();
    }

    public void SkipShop()
    {
        choiceManager.EndChoice();
        statsPlayer.InvokeGainPV(SOShop.currentGainPV);
        SOShop.currentGainPV -= 1;
        if (SOShop.currentGainPV < 1) SOShop.currentGainPV = 1;
        SkipText.text = "+" + SOShop.currentGainPV + "  <sprite name=heal 1>";

    }
}
