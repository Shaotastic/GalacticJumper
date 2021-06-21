using com.shephertz.app42.paas.sdk.csharp.game;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public enum LeaderboardPage
    {
        Top50 = 0,
        Personal
    }

    LeaderboardPage CurrentPage;

    [SerializeField] LeaderBoardPanel panelReference;
    [SerializeField] TextMeshProUGUI m_TextBox;
    [SerializeField] List<LeaderBoardPanel> panels;
    [SerializeField] Transform parentTransform;
    [SerializeField] Button m_ExitButton;
    [SerializeField] TextMeshProUGUI m_PlayerHighScoreText;
    [SerializeField] Button m_RetryButton;
    [SerializeField] GameObject m_ConnectingText;

    public List<Game.Score> m_Scores;

    private bool m_DataCollected;

    [SerializeField] private float yOffset = 30;

    private bool m_ShowBoard;

    public delegate void ExitBoard();

    public event ExitBoard HideLeaderboard;

    public static LeaderBoard Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = GetComponent<LeaderBoard>();

        m_ExitButton.onClick.AddListener(HideBoard);
        m_RetryButton.onClick.AddListener(Initialize);
    }

    // Start is called before the first frame update
    void Start()
    {
        panels = new List<LeaderBoardPanel>();

        if (GameManager.Instance.HasInternet())
        {
            Initialize();
        }
        else
        {
            m_TextBox.gameObject.SetActive(true);
        }
        m_ConnectingText.SetActive(false);
        Menu.Instance.showBoard += ShowBoard;
        m_ShowBoard = false;

    }

    void Initialize()
    {
        App24Leaderboard.GetTop50Rankers();
        StartCoroutine(InitializeBoard());
        StartCoroutine(AssigningLeaderboard());
        StartCoroutine(ButtonTimer());
    }

    IEnumerator ButtonTimer()
    {
        m_TextBox.gameObject.SetActive(false);
        m_RetryButton.gameObject.SetActive(false);
        m_ConnectingText.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        if (!m_DataCollected)
        {
            m_TextBox.gameObject.SetActive(true);
            m_RetryButton.gameObject.SetActive(true);
            m_ConnectingText.SetActive(false);
        }
    }

    void UpdateBoard()
    {
        App24Leaderboard.GetTop50Rankers();
        StartCoroutine(AssigningLeaderboard());
    }

    IEnumerator InitializeBoard()
    {
        yield return new WaitUntil(() => m_DataCollected);
        Vector3 position = new Vector3(0, -70);
        for (int i = 0; i < m_Scores.Count; i++)
        {
            LeaderBoardPanel temp = Instantiate(panelReference, parentTransform);
            temp.SetPosition(position);
            panels.Add(temp);

            //string rank = (i + 1).ToString() + ".";
            //temp.AssignPanel(rank, m_Scores[i].GetUserName(), m_Scores[i].GetValue().ToString());
            position = new Vector3(position.x, position.y - yOffset);
        }
    }

    IEnumerator AssigningLeaderboard()
    {
        yield return new WaitUntil(() => m_DataCollected);
        for (int i = 0; i < m_Scores.Count; i++)
        {
            string rank = (i + 1).ToString() + ".";
            panels[i].AssignPanel(rank, m_Scores[i].GetUserName(), m_Scores[i].GetValue().ToString());
            //position = new Vector3(position.x, position.y - yOffset);
        }
    }

    void ShowBoard()
    {
        if (!m_ShowBoard)
        {
            m_PlayerHighScoreText.text = "Highest Score: <size=150%>" + GameManager.Instance.GetHighScore().ToString() + "</size>";
            //UpdateBoard();
            UpdateScores();
            transform.DOLocalMoveX(0, 0.2f);
            m_ShowBoard = true;
        }
    }

    void HideBoard()
    {
        if (m_ShowBoard)
        {
            HideLeaderboard();
            transform.DOLocalMoveX(-1220, 0.2f);
            m_ShowBoard = false;
            if (!GameManager.Instance.m_GameStarted)
                Menu.Instance.ToggleMenuButtons(true);
            else
                Menu.Instance.ToggleMenuButtons(true, false);
        }
    }

    void UpdateScores()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            string rank = (i + 1).ToString() + ".";
            panels[i].AssignPanel(rank, m_Scores[i].GetUserName(), m_Scores[i].GetValue().ToString());

            if (m_Scores[i].GetUserName() == App24Leaderboard.m_UserID)
            {
                panels[i].AssignColor(Color.green);
            }
            else
                panels[i].AssignColor(Color.white);
        }
    }

    public void CanGetData(IList<Game.Score> scores)
    {
        m_DataCollected = true;
        m_ConnectingText.SetActive(false);
        m_Scores = scores.ToList();
        UpdateScores();
    }

}
