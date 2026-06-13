using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UnityConsent;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private float _sessionStartTime;
    private bool _initialized;

    //  Init 

    private async void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
                await UnityServices.InitializeAsync();

            EndUserConsent.SetConsentState(new ConsentState
            {
                AnalyticsIntent = ConsentStatus.Granted
            });

            _initialized = true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[Analytics] Init failed: {e.Message}");
        }
    }

    private void Start() => _sessionStartTime = Time.realtimeSinceStartup;

    private void OnApplicationQuit() => RecordSessionEnd("quit");

    //  Helper 

    private bool CanRecord()
    {
        if (!_initialized || UnityServices.State != ServicesInitializationState.Initialized)
        {
            Debug.LogWarning("[Analytics] Not ready, skipping event.");
            return false;
        }
        return true;
    }

    private void Send(CustomEvent evt, bool flush = false)
    {
        try
        {
            AnalyticsService.Instance.RecordEvent(evt);
            if (flush) AnalyticsService.Instance.Flush();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[Analytics] Failed to send '{evt.Name}': {e.Message}");
        }
    }

    //  Events 

    [Button("Test: Session End")]
    public void RecordSessionEnd(string outcome)
    {
        if (!CanRecord()) return;

        Send(new CustomEvent("session_end")
        {
            { "outcome",      outcome                                         },
            { "time_played",  Time.realtimeSinceStartup - _sessionStartTime  },
        }, flush: true); // flush on session end
    }

    public void RecordDeath(string killerName, int floor, float hpRemaining, List<string> pieceIds)
    {
        if (!CanRecord()) return;

        Send(new CustomEvent("player_death")
        {
            { "killer_name",   killerName                  },
            { "floor",         floor                       },
            { "hp_remaining",  hpRemaining                 },
            { "pieces",        string.Join(",", pieceIds)  },
        });

        RecordSessionEnd("death");
    }

    public void RecordVictory(int floor, List<string> pieceIds)
    {
        if (!CanRecord()) return;

        Send(new CustomEvent("player_victory")
        {
            { "floor",        floor                       },
            { "pieces",       string.Join(",", pieceIds)  },
            { "time_played",  Time.realtimeSinceStartup - _sessionStartTime },
        });

        RecordSessionEnd("win");
    }
}