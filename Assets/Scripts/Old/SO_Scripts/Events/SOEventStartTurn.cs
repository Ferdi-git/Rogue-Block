using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/SOEventStartTurn")]
public class SOEventStartTurn : ScriptableObject
{
    public event Action NextTurn;

    public void InvokeNextTurn()
    {
        NextTurn?.Invoke();
    }

}
