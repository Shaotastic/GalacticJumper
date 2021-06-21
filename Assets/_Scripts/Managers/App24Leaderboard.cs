using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;
using com.shephertz.app42.paas.sdk.csharp.log;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.user;
using System;
using System.Collections;
using UnityEngine;

public class App24Leaderboard : MonoBehaviour
{
    private const string m_APIKey = "96f57ee8744fb0341e372c5ec5947a6f62e640872e9b5efd9a04ea53e221a51a";
    private const string m_SecretKey = "6126c8f489d0acd690683df1c7690b7b2537f3867a4813afd123dee2b98573a7";
    private const string m_PlayerStatsID = "5efac687e4b030854f43b307";

    public static ServiceAPI serviceAPI;

    public static UserService m_UserService;

    public static StorageService m_StorageService, m_PlayerStorageService;

    public static ScoreBoardService m_ScoreBoard;

    public static LogService m_LogService;

    public static string m_UserID, m_ScoreID;
    private static int m_PlayerID;

    private void Awake()
    {
        serviceAPI = new ServiceAPI(m_APIKey, m_SecretKey);

        m_UserService = serviceAPI.BuildUserService();
        m_StorageService = serviceAPI.BuildStorageService();
        m_PlayerStorageService = serviceAPI.BuildStorageService();
        m_ScoreBoard = serviceAPI.BuildScoreBoardService();
        m_LogService = serviceAPI.BuildLogService();

        m_UserID = PlayerPrefs.GetString("Username", String.Empty);
        m_ScoreID = PlayerPrefs.GetString("ScoreID", String.Empty);

        if (m_UserID != null)
            m_ScoreBoard.GetHighestScoreByUser(Application.productName, m_UserID, new GetHighScore());
    }

    public static void SaveScoreToServer(double score)
    {

        if (m_ScoreID == String.Empty)
            m_ScoreBoard.SaveUserScore(Application.productName, m_UserID, (double)score, new SaveScoreToServer());
        else
        {
            m_ScoreBoard.EditScoreValueById(m_ScoreID, (double)score, new SaveScoreToServer());
        }
    }

    public static void CreateNewUser(string newName)
    {
        m_UserID = newName;
        m_UserService.CreateUser(m_UserID, "***********", m_UserID + "@notReal.ohwell", new CreateUser());
        m_ScoreID = String.Empty;
        PlayerPrefs.SetString("Username", m_UserID);
    }

    public static void SetUserName(string name, int type)
    {
        m_UserID = name;
        PlayerPrefs.SetString("Username", m_UserID);
        m_ScoreID = String.Empty;
        PlayerPrefs.SetInt("UserNameAssigned", type);
    }

    public static void GetTop50Rankers()
    {
        int max = 50;
        m_ScoreBoard.GetTopNRankers(Application.productName, max, new GetTop50Scores());
    }
    public static IEnumerator DoesUserExist(string theName)
    {
        m_UserService.GetUser(theName, new CheckUserName());
        yield return new WaitForSeconds(0.5f);
        if (CheckUserName.check)
        {
            yield return false;
        }
        else
        {
            yield return true;
        }
    }
}

public class CreateUser : App42CallBack
{
    public void OnException(Exception ex)
    {
        App42Log.Console("Exception : " + ex);
    }

    public void OnSuccess(object response)
    {
        User user = (User)response;

        App42Log.Console("This User: " + user + " has signed in.");
    }
}


public class SaveScoreToServer : App42CallBack
{
    public void OnException(Exception ex)
    {
        App42Log.Console("Exception : " + ex);
    }

    public void OnSuccess(object response)
    {
        Game game = (Game)response;
        if (App24Leaderboard.m_ScoreID == String.Empty)
        {
            for (int i = 0; i < game.GetScoreList().Count; i++)
            {
                if (App24Leaderboard.m_UserID == game.GetScoreList()[i].GetUserName())
                {
                    PlayerPrefs.SetString("ScoreID", game.GetScoreList()[i].GetScoreId());

                }
                App42Log.Console("userName is : " + game.GetScoreList()[i].GetUserName());
                App42Log.Console("score is : " + game.GetScoreList()[i].GetValue());
                App42Log.Console("scoreId is : " + game.GetScoreList()[i].GetScoreId());
            }
        }
    }
}

public class GetHighScore : App42CallBack
{
    public void OnException(Exception ex)
    {
        App42Log.Console("Exception : " + ex);
    }

    public void OnSuccess(object response)
    {
        if (GameManager.Instance.HasInternet())
        {
            Game game = (Game)response;
            for (int i = 0; i < game.GetScoreList().Count; i++)
            {
                if (App24Leaderboard.m_UserID == game.GetScoreList()[i].GetUserName())
                {
                    GameManager.Instance.SetUserHighScore(game.GetScoreList()[i].GetValue());
                }
            }
        }
    }
}


public class GetTop50Scores : App42CallBack
{
    public void OnException(Exception ex)
    {
        App42Log.Console("Exception : " + ex);
    }

    public void OnSuccess(object response)
    {
        Game game = (Game)response;
        LeaderBoard.Instance.CanGetData(game.GetScoreList());
    }
}

public class CheckUserName : App42CallBack
{
    public static bool check;

    public void OnException(Exception ex)
    {
        check = false;
        Debug.Log("Its not working");
    }

    public void OnSuccess(object response)
    {
        User user = (User)response;


        check = true;
        Debug.Log("Exist idiot");

    }
}
