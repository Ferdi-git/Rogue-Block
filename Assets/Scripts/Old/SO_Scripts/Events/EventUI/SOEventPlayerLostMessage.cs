using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Old/SOEventPlayerLostMessage")]
public class SOEventPlayerLostMessage : ScriptableObject
{
    public event Action ActiveLostMessage;


    public void InvokeActiveLostMessage()
    {
        ActiveLostMessage?.Invoke();
    }
}
