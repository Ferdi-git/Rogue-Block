using System;
using UnityEngine;
[CreateAssetMenu]
public class SOEventPlayerLostMessage : ScriptableObject
{
    public event Action ActiveLostMessage;


    public void InvokeActiveLostMessage()
    {
        ActiveLostMessage?.Invoke();
    }
}
