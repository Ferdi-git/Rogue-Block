using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventPlayer : ScriptableObject
{
    public event Action<int> GainPV;
    public event Action<int> LoosePV;

    public event Action<int> GainMana;
    public event Action<int> LooseMana;

    public event Action<int> GainShield;
    public event Action<int> LooseShield;

    public void InvokePlayerGainPV(int amount) { GainPV?.Invoke(amount); }
    public void InvokePlayerLoosePV(int amount) { LoosePV?.Invoke(amount); }
    public void InvokePlayerGainMana(int amount) { GainMana?.Invoke(amount); }
    public void InvokePlayerLooseMana(int amount) { LooseMana?.Invoke(amount); }
    public void InvokePlayerGainShield(int amount) { GainShield?.Invoke(amount); }
    public void InvokePlayerLooseShield(int amount) { LooseShield?.Invoke(amount); }
}
