using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Old/SOEventPLayerWinMessage")]
public class SOEventPLayerWinMessage : ScriptableObject
{
    public event Action WinMessageEvent;

    public void InvokeWinMessageEvent()
    {
        WinMessageEvent?.Invoke();
    }
}
