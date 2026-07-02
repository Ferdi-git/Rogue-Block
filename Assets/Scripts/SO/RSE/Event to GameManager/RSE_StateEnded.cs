using System;
using UnityEngine;
[CreateAssetMenu (fileName = "RSE_StateEnded" , menuName = "SO/RSE/StateEnded")]
public class RSE_StateEnded : ScriptableObject
{
    public event Action stateEndedEvent;

    public void InvokeStateEnded ()
    {
        stateEndedEvent.Invoke ();
    }
}
