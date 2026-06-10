using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventEndPlayerTurn : ScriptableObject
{
    public event Action EndTurn;

    public void InvokeEndTurn()
    {
        EndTurn?.Invoke();
    }

}
