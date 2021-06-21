using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public enum PageStates
    {
        LeaderBoard = -1,
        Main = 0,
        Options = 1
    }

    [SerializeField] public PageStates m_Pages;

    private const string firstTime = "FIRSTTIME";
    // Use this for initialization
    private int m_FirstTime;
    [SerializeField] GameObject m_NewUserPage;
    [SerializeField] Button m_PlayButton;
    [SerializeField] Button m_LeaderboardButton;
    [SerializeField] Button m_OptionButton;
    [SerializeField] Button m_ResetButton;

    public delegate void ShowLeaderBoard();
    public delegate void ShowOptions();

    public event ShowLeaderBoard showBoard;
    public event ShowOptions showOptions;
    public event ShowOptions hideOptions;

    public static Menu Instance;

    public const float m_xDirection = 5;

    private void Awake()
    {
        if (Instance == null)
            Instance = GetComponent<Menu>();

        //GameManager.Instance.OnDeath += () => m_LeaderboardButton.gameObject.SetActive(true);
        //GameManager.Instance.OnDeath += () => m_OptionButton.gameObject.SetActive(true);
        GameManager.Instance.OnReset += () => m_LeaderboardButton.gameObject.SetActive(false);
        GameManager.Instance.OnReset += () => m_OptionButton.gameObject.SetActive(false);
        m_LeaderboardButton.onClick.AddListener(ShowLeaderBoardScreen);
        m_OptionButton.onClick.AddListener(ShowOptionsScreen);
        m_LeaderboardButton.onClick.AddListener(App24Leaderboard.GetTop50Rankers);
        m_PlayButton.onClick.AddListener(() => { GameManager.Instance.GameStart();});
        //AdManager.Instance.AdComplete += () => ToggleMenuButtons(true, false);
        FlashScreen.AtFlash += () => m_LeaderboardButton.gameObject.SetActive(true);
        FlashScreen.AtFlash += () => m_OptionButton.gameObject.SetActive(true);
        AdManager.Instance.AdFinished += () => ToggleMenuButtons(false, false);
        //Check if user has a name
        //m_NewUserPage.SetActive(false);

    }

    void Start()
    {
        StartCoroutine(CheckFirstTime());
        ToggleMenuButtons(true);
        m_Pages = 0;
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    void ShowLeaderBoardScreen()
    {
        showBoard();
        m_Pages = PageStates.LeaderBoard;
        ToggleMenuButtons(false, false);
    }

    void ShowOptionsScreen()
    {
        showOptions();
        m_Pages = PageStates.Options;
        ToggleMenuButtons(false, false);
    }

    public void ToggleMenuButtons(bool value, bool showPlayButton = true)
    {
        m_LeaderboardButton.gameObject.SetActive(value);
        m_PlayButton.transform.parent.gameObject.SetActive(showPlayButton);
        m_OptionButton.gameObject.SetActive(value);
        if (value)
            m_Pages = PageStates.Main;
    }

    public void HideOptionScreen()
    {
        hideOptions();
        m_Pages = PageStates.Main;
    }

    public void ShowLeaderboardButton()
    {
        m_LeaderboardButton.gameObject.SetActive(true);
    }

    public bool IsFirstTime
    {
        get {
            if (m_FirstTime == 1)
                return true;

            return false;
        }
    }

    IEnumerator CheckFirstTime()
    {
        yield return new WaitUntil(() => LoadingScene.m_Loaded);
        if (LoadingScene.m_Loaded)
        {
            m_FirstTime = PlayerPrefs.GetInt(firstTime);

            int userNameCheck = PlayerPrefs.GetInt("UserNameAssigned", 0);

            if (m_FirstTime != 1 || userNameCheck != 1)
            {
                m_FirstTime = 1;
                PlayerPrefs.SetInt(firstTime, m_FirstTime);
                PlayerPrefs.SetString("Highscore", "0");
                //PlayerPrefs.SetInt("UserNameAssigned", 0);
                //App24Leaderboard.AddUserToServer();
                m_NewUserPage.SetActive(true);
                Debug.Log("Is Users first time");
            }
            else if (userNameCheck == 2)
            {
                m_NewUserPage.SetActive(true);
            }
            Debug.Log("If statements executed");
        }
    }
}
