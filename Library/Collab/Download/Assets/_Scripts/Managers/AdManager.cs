using com.shephertz.app42.paas.sdk.csharp.game;
using System;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public static AdManager Instance;

#if UNITY_IOS
    private string m_GameID = "3591059";
#elif UNITY_ANDROID
    private string m_GameID = "3591058";//"1486550";
#else
    private string m_GameID = "3591058";
#endif

    private const string m_RewardPlacementID = "rewardedVideo";

    private const string m_ExtraLifePlacementID = "ExtraLife";

    [SerializeField] Button m_ExtraLifeButton;

    public delegate void AdFinishReset();

    public event AdFinishReset AdFinished, AdBegin, AdComplete;

    public void Awake()
    {
        if (Instance == null)
            Instance = GetComponent<AdManager>();
    }

    void Start()
    {
        if (GameManager.Instance.HasInternet())
        {
            // Set interactivity to be dependent on the Placement’s status:
            m_ExtraLifeButton.interactable = Advertisement.IsReady(m_ExtraLifePlacementID);

            // Map the ShowRewardedVideo function to the button’s click listener:
            if (m_ExtraLifeButton)
                m_ExtraLifeButton.onClick.AddListener(ShowExtraLifeAd);

            // Initialize the Ads listener and service:
            Advertisement.AddListener(this);
            Advertisement.Initialize(m_GameID, true);

            Menu.Instance.showBoard += (() => {
                if (GameManager.Instance.m_GameStarted)
                    m_ExtraLifeButton.transform.parent.gameObject.SetActive(false);
            });

            LeaderBoard.Instance.HideLeaderboard += (() => {
                if (GameManager.Instance.m_GameStarted)
                    m_ExtraLifeButton.transform.parent.gameObject.SetActive(true);
            });
        }
        Reset();//m_ExtraLifeButton.gameObject.SetActive(false);
                //Debug.Log(m_GameID);

        Menu.Instance.showOptions += (() => {
            if (GameManager.Instance.m_GameStarted)
                m_ExtraLifeButton.transform.parent.gameObject.SetActive(false);
        });
        
        Menu.Instance.hideOptions += (() => {
            if (GameManager.Instance.m_GameStarted)
                m_ExtraLifeButton.transform.parent.gameObject.SetActive(true);
        });

        FlashScreen.AtFlash += EnableExtraLifeButton;
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == m_ExtraLifePlacementID)
        {
            m_ExtraLifeButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        GameManager.Instance.m_Pause = false;


        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            switch (placementId)
            {
                case m_ExtraLifePlacementID:
                    Debug.Log("YOU WON!!!");
                    GameManager.Instance.m_ExtraLife = true;
                    //GameManager.Instance.m_ResetResults = false;
                    m_ExtraLifeButton.transform.parent.gameObject.SetActive(false);
                    AdFinished();
                    break;
                case m_RewardPlacementID:
                    AdComplete();
                    break;
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            Debug.Log("YOU LOOOOSE");// Do not reward the user for skipping the ad.
            switch(placementId)
            {                    
                case m_RewardPlacementID:
                    AdComplete();
                    break;
            }
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");

            switch (placementId)
            {
                case m_ExtraLifePlacementID:
                    AdComplete();
                    break;
                case m_RewardPlacementID:
                    AdComplete();
                    break;
            }
        }
        //GameManager.Instance.AdReset();
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
        GameManager.Instance.m_Pause = false;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
        AdBegin();
        GameManager.Instance.m_Pause = true;
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(m_RewardPlacementID);
    }

    public void ShowExtraLifeAd()
    {
        Advertisement.Show(m_ExtraLifePlacementID);      
    }    

    public void Reset()
    {
        m_ExtraLifeButton.transform.parent.gameObject.SetActive(false);
    }

    void EnableExtraLifeButton()
    {
        //Check if we have internet, check if the extra life is enabled
        if(GameManager.Instance.HasInternet())
        {
            if (!GameManager.Instance.m_ExtraLife)
            {
                m_ExtraLifeButton.transform.parent.gameObject.SetActive(true);
                OnUnityAdsReady(m_ExtraLifePlacementID);
            }
        }
    }
}
