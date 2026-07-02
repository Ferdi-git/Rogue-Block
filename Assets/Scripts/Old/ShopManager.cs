using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject grid;
    [SerializeField] ChoiceManager shopM;
    [SerializeField] SOEventState eventState;

    private bool _isOpen = false;

    private void OnEnable()
    {
        eventState.StartShoping += OpenShop;
    }
    private void OnDisable()
    {
        eventState.StartShoping -= OpenShop;
    }

    private void OpenShop()
    {
        if (_isOpen) return; 
        _isOpen = true;

        DOTween.Kill(grid.transform);
        DOTween.Kill(shopM.transform);

        grid.SetActive(false);
        shopM.gameObject.SetActive(true);
        shopM.GeneratePiece();
    }

    public void CloseShop()
    {
        if (!_isOpen) return; 
        _isOpen = false;

        DOTween.Kill(grid.transform);
        DOTween.Kill(shopM.transform);

        grid.SetActive(true);
        shopM.gameObject.SetActive(false);
        eventState.InvokeEndOfShoping();
    }
}