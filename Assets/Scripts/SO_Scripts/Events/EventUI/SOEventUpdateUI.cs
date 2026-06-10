using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventUpdateUI : ScriptableObject
{
    public event Action UpdateUI;


    public void InvokeUpdateUI()
    {
        UpdateUI?.Invoke();
    }
}
