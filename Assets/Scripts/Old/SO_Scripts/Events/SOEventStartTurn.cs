using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventStartTurn : ScriptableObject
{
    public event Action NextTurn;

    public void InvokeNextTurn()
    {
        NextTurn?.Invoke();
    }

}
