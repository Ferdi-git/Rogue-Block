using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventVisuelEffect : ScriptableObject 
{
    public event Action<VisuelAttakData> EffectAtkEnemy;
    public event Action<VisuelAttakData> EffectEnemyDealAtk;
    public event Action<VisuelAttakData> EffectGainMana;
    public void InvokeEffectAtkEnemy(VisuelAttakData data) { EffectAtkEnemy?.Invoke(data); }
    public void InvokeEffectEnemyDealAtk(VisuelAttakData data) { EffectEnemyDealAtk?.Invoke(data); }
    public void InvokeEffectGainMana(VisuelAttakData data) { EffectGainMana?.Invoke(data); }
   
}

public class VisuelAttakData
{
    public int nbrDMG;
    public Vector3 posAttacker;
    public Action eventEndVisuel;

}