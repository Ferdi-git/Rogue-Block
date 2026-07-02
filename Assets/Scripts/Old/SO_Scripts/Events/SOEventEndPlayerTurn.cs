using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/SOEventEndPlayerTurn")]
public class SOEventEndPlayerTurn : ScriptableObject
{
    public event Action EndTurn;

    public void InvokeEndTurn()
    {
        EndTurn?.Invoke();
    }

}
