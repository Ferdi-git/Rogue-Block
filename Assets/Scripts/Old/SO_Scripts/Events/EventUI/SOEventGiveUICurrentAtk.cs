using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Old/SOEventGiveUICurrentAtk")]
public class SOEventGiveUICurrentAtk : ScriptableObject
{
    public event Action<int> GiveUICurrentAtk;


    public void InvokeGiveUICurrentAtk(int index)
    {
        GiveUICurrentAtk?.Invoke(index);
    }
}
