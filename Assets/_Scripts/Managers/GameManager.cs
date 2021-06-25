using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static double m_Highscore;
    public bool DifficultyHard;
    public TextMeshProUGUI m_HighscoreText;

    //public bool m_ResetResults;
    private double m_Score = 0;
    private int m_DeathCount = 0;
    private int m_PlanetCount = 0;
    [SerializeField] private Player player;

    private static bool m_SpawnGasPlanet;
    public bool m_GameStarted;

    public bool m_ExtraLife = false;

    public delegate void PlayerDeath();

    public delegate void OnGameStart();

    public delegate void ScoreFunction();

    public event ScoreFunction OnScore;

    public event PlayerDeath OnDeath;

    public event PlayerDeath OnReset;

    public bool m_Pause = false;


    [SerializeField] private float m_PlanetDecaySpeed = 5;
    private float m_SpeedMultiplier = 1;
    private float m_DecaySpeedMultiplier = 1;

    public bool m_IsPlaying = false;

    [SerializeField] private float m_CurrentDecaySpeed;

    private void Awake()
    {
        if (Instance == null)
            Instance = GetComponent<GameManager>();

        m_Pause = false;
    }

    // Use this for initialization
    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //if (!HasInternet())
        //{
        double highScore = double.Parse(PlayerPrefs.GetString("Highscore", "0"));

        SetUserHighScore(highScore);
        //}

        AdManager.Instance.AdComplete += () => { m_IsPlaying = true; };
        AdManager.Instance.AdFinished += () => { m_IsPlaying = true; };
        m_HighscoreText.gameObject.SetActive(false);
        //m_ResetResults = false;
        AdManager.Instance.AdFinished += () => { m_DeathCount = 0; };
        m_DeathCount = 0;
        SetDecaySpeed(m_PlanetDecaySpeed * m_DecaySpeedMultiplier);
        m_CurrentDecaySpeed = m_PlanetDecaySpeed * m_DecaySpeedMultiplier;

    }

    private void ScoreTracker()
    {
        switch (m_Score)
        {
            case 10:
                SetMultiplierDecay(0.9f);
                return;
            case 20:
                SetMultiplierDecay(0.8f);
                return;
            case 40:
                SetMultiplierDecay(0.7f);
                return;
            case 80:
                SetMultiplierDecay(0.65f);
                return;
            case 100:
                SetMultiplierDecay(0.5f);
                return;
            default:
                return;
        }
    }

    public void DisplayResults()
    {
        if (m_Score > m_Highscore)
        {
            SaveResults(m_Score);
            m_HighscoreText.text = "New Highscore: \n" + m_Score;
        }
        else
            m_HighscoreText.text = "Final Score:\n" + m_Score;// + "\nHighscore: " + m_Highscore;

    }

    private void SaveResults(double score)
    {
        SetUserHighScore(score);
        App24Leaderboard.SaveScoreToServer(score);
    }

    public void Reset()
    {
        //m_ResetResults = false;
        m_PlanetCount = 0;
        m_SpeedMultiplier = 1;
        SetMultiplierDecay(1);
        m_IsPlaying = true;
        m_ExtraLife = false;
        ResetScore();
        OnReset();
        //player.Reset();
    }

    public void AddScore()
    {
        m_PlanetCount++;

        switch (m_ExtraLife)
        {
            case false:
                m_Score++;
                break;
            case true:
                m_Score += m_PlanetCount % 2;
                break;
        }
        ScoreTracker();
        OnScore();

    }

    void ResetScore()
    {
        if (!m_ExtraLife)
            m_Score = 0;
        OnScore();
    }

    public double GetScore()
    {
        return m_Score;
    }

    public int GetPlanetCount()
    {
        return m_PlanetCount;
    }

    public void AddDeath()
    {
        m_DeathCount++;
        if (m_DeathCount > 0)
            if (m_DeathCount % 10 == 0)
                AdManager.Instance.ShowRewardedAd();//Advertisement.Show(myPlacementId);

        //m_ResetResults = true;
        m_IsPlaying = false;
        OnDeath();
        DisplayResults();
    }

    public void ActivateGasPlanet()
    {
        if (!m_SpawnGasPlanet)
            m_SpawnGasPlanet = true;
    }

    public bool CheckGasPlanetSpawn()
    {
        if (m_SpawnGasPlanet)
        {
            m_SpawnGasPlanet = false;
            return true;
        }
        return false;
    }

    public void GameStart()
    {
        m_GameStarted = true;
        m_IsPlaying = true;
    }

    public void SetUserHighScore(double score)
    {
        m_Highscore = score;
        PlayerPrefs.SetString("Highscore", score.ToString());
    }

    public double GetHighScore()
    {
        return m_Highscore;
    }

    public float GetPlanetDecaySpeed()
    {
        return m_CurrentDecaySpeed;// m_PlanetDecaySpeed * m_DecaySpeedMultiplier;
    }

    public float GetSpeedMultiplier()
    {
        return m_SpeedMultiplier;
    }

    public bool HasInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        return true;
    }

    public Player GetPlayer()
    {
        return player;
    }

    private void SetDecaySpeed(float val)
    {
        m_CurrentDecaySpeed = val;
    }

    private void SetMultiplierDecay(float mul)
    {
        m_DecaySpeedMultiplier = mul;
        SetDecaySpeed(m_PlanetDecaySpeed * m_DecaySpeedMultiplier);
    }
}
