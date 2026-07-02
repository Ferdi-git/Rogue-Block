using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/SOEventUpdateUI")]
public class SOEventUpdateUI : ScriptableObject
{
    public event Action UpdateUI;


    public void InvokeUpdateUI()
    {
        UpdateUI?.Invoke();
    }
}
