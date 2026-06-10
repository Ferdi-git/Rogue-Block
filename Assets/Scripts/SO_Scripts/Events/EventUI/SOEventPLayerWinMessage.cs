using System;
using UnityEngine;

[CreateAssetMenu]
public class SOEventPLayerWinMessage : ScriptableObject
{
    public event Action WinMessageEvent;

    public void InvokeWinMessageEvent()
    {
        WinMessageEvent?.Invoke();
    }
}
