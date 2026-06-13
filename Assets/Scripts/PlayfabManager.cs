using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Sirenix.OdinInspector;
public class PlayfabManager : MonoBehaviour
{
    private float sessionStartTime;

    private void Start()
    {
        sessionStartTime = Time.realtimeSinceStartup; 

        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }


    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login");
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Success Data sent");
    }

    void OnStatSend(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Success Stat sent");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("PLAYFAB Error");
        Debug.Log(error.GenerateErrorReport());
    }

    [Button]
    private void SaveInventory(SoSaveInventory saveInventory)
    {
        string toSave = "";
        string posToSave = "";
        string rotToSave = "";
        for (int i = 0; i < saveInventory.listBoardPiecesExist.Count; i++)
        {
            toSave += saveInventory.listBoardPiecesExist[i].soPieces.name;
            posToSave += saveInventory.piecesPos[i].ToString();
            rotToSave += saveInventory.piecesRot[i].ToString();
            toSave += "|";
            posToSave += "|";
            rotToSave += "|";
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"PieceInInventory", toSave},
                {"PosPieceInInventory", posToSave},
                {"RotPieceInInventory", rotToSave}

            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    [Button]
    private void SaveSessionTime()
    {
        int sessionSeconds = Mathf.RoundToInt(Time.realtimeSinceStartup - sessionStartTime);

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
        {
            new StatisticUpdate { StatisticName = "TimePlayedTotal",    Value = sessionSeconds },
            new StatisticUpdate { StatisticName = "TimePlayedLongest",     Value = sessionSeconds },
            new StatisticUpdate { StatisticName = "TimePlayedShortest", Value = sessionSeconds },
        }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnStatSend, OnError);
    }

    private void OnApplicationQuit()
    {
        SaveSessionTime();
    }

}
