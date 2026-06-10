using UnityEngine;
using System;
[CreateAssetMenu]
public class SOEventGiveUICurrentAtk : ScriptableObject
{
    public event Action<int> GiveUICurrentAtk;


    public void InvokeGiveUICurrentAtk(int index)
    {
        GiveUICurrentAtk?.Invoke(index);
    }
}
