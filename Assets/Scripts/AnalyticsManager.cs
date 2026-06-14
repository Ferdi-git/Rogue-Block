using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UnityConsent;

public class AnalyticsManager : MonoBehaviour
{
    [SerializeField] SoSaveInventory saveInventory;
    public static AnalyticsManager Instance { get; private set; }

    private float _sessionStartTime;
    private int enemiesKilled;
    private float maxDamage;
    private List<int> turnsPerEnemy = new();
    private List<float> timePerEnemy = new();
    private int totalTurns;
    private float _turnStartTime;
    private float totalTurnTime;

    private bool _initialized;

    private async void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
                await UnityServices.InitializeAsync();
            EndUserConsent.SetConsentState(new ConsentState { AnalyticsIntent = ConsentStatus.Granted });
            _initialized = true;
        }
        catch (Exception e) { Debug.LogError($"[Analytics] Init failed: {e.Message}"); }
    }

    private void Start() => _sessionStartTime = Time.realtimeSinceStartup;
    private void OnApplicationQuit() => RecordSessionEnd("quit");

    private bool CanRecord() =>
        _initialized && UnityServices.State == ServicesInitializationState.Initialized;

    private void Send(CustomEvent evt, bool flush = false)
    {
        try
        {
            AnalyticsService.Instance.RecordEvent(evt);
            if (flush) AnalyticsService.Instance.Flush();
        }
        catch (Exception e) { Debug.LogWarning($"[Analytics] Failed {e.Message}"); }
    }

    [Button("Test: Session End")]
    public void RecordSessionEnd(string outcome)
    {
        if (!CanRecord()) return;
        Send(new CustomEvent("session_end")
        {
            { "outcome",     outcome },
            { "time_played", Time.realtimeSinceStartup - _sessionStartTime },
        }, flush: true);
    }


    public void RecordDeath(string killerName, int floor, float hpRemaining)
    {
        Send(new CustomEvent("player_death")
    {
        { "killer_name",    killerName                              },
        { "floor",          floor                                   },
        { "hp_remaining",   hpRemaining                            },
        { "pieces",         GetInventory(saveInventory)[0]         },
        { "enemies_killed", enemiesKilled                         },
        { "max_damage",     maxDamage                             },
        { "turns_per_enemy",string.Join(",", turnsPerEnemy)       },
        { "time_per_enemy", string.Join(",", timePerEnemy)        },
        { "avg_turn_time",  totalTurns > 0 ? totalTurnTime / totalTurns : 0f },
    });
        RecordSessionEnd("Lose");
    }



    public void RecordVictory(int floor, float hpRemaining)
    {
        Send(new CustomEvent("player_victory")
    {
        { "floor",          floor                                   },
        { "hp_remaining",   hpRemaining                            },
        { "pieces",         GetInventory(saveInventory)[0]         },
        { "enemies_killed", enemiesKilled                         },
        { "max_damage",     maxDamage                             },
        { "turns_per_enemy",string.Join(",", turnsPerEnemy)       },
        { "time_per_enemy", string.Join(",", timePerEnemy)        },
        { "avg_turn_time",  totalTurns > 0 ? totalTurnTime / totalTurns : 0f },
    });
        RecordSessionEnd("Win");
    }


    private string[] GetInventory(SoSaveInventory saveInventory)
    {
        string pieceNames = "";
        string posToSave = "";
        string rotToSave = "";
        for (int i = 0; i < saveInventory.listBoardPiecesExist.Count; i++)
        {
            pieceNames += saveInventory.listBoardPiecesExist[i].soPieces.name;
            posToSave += saveInventory.piecesPos[i].ToString();
            rotToSave += saveInventory.piecesRot[i].ToString();
            pieceNames += "|";
            posToSave += "|";
            rotToSave += "|";
        }

        return new string[] { pieceNames, posToSave, rotToSave };

    }




    public void OnTurnStart()
    {
        _turnStartTime = Time.realtimeSinceStartup;
        totalTurns++;
    }

    public void OnTurnEnd()
    {
        totalTurnTime += Time.realtimeSinceStartup - _turnStartTime;
    }

    public void OnEnemyKilled(int turns, float timeSeconds)
    {
        enemiesKilled++;
        turnsPerEnemy.Add(turns);
        timePerEnemy.Add(MathF.Round(timeSeconds, 1));
    }

    public void OnPieceDamage(float damage)
    {
        if (damage > maxDamage) maxDamage = damage;
    }


}