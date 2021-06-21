using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

#if UNITY_IOS
    float m_yPosition = -120;
#else
    float m_yPosition = -100;
#endif


    [SerializeField] TextMeshProUGUI m_ScoreText;
    [SerializeField] TextMeshProUGUI m_HighScoreText;
    [SerializeField] TextMeshProUGUI m_SpeedIncreaseText;
    //[SerializeField] Image m_FuelBar;
    [SerializeField] Button m_ResetButton;
    Sequence sequence;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        AdManager.Instance.AdComplete += OnDeath;
        Menu.Instance.showBoard += HideResetButton;
        Menu.Instance.showOptions += HideResetButton;
        Menu.Instance.hideOptions += ShowResetButton;
        LeaderBoard.Instance.HideLeaderboard += ShowResetButton;
        //GameManager.Instance.OnDeath += OnDeath;
        GameManager.Instance.OnReset += Reset;
        GameManager.Instance.OnScore += SetScoreText;
        AdManager.Instance.AdFinished += Reset;
        FlashScreen.AtFlash += OnDeath;

        m_ResetButton.onClick.AddListener(() =>
        {
            PlanetObjectPoolManager.Instance.ResetPlanet();
            GameManager.Instance.Reset();
            //Player Reset
            AdManager.Instance.Reset();
            CameraFollow.Instance.ResetCamera();
        });

        //GameManager.Instance.OnScoreTicker += SpeedIncrease;

        m_ScoreText.rectTransform.anchoredPosition = new Vector2(-300, m_yPosition);

        m_SpeedIncreaseText.DOFade(0, 0);
    }

    private void Initialize()
    {
        m_ResetButton.transform.parent.gameObject.SetActive(false);
        //m_FuelBar.gameObject.SetActive(false);
        m_ScoreText.gameObject.SetActive(false);
    }

    public void SetScoreText()
    {
        m_ScoreText.text = GameManager.Instance.GetScore().ToString();
    }

    void OnDeath()
    {
        //m_FuelBar.gameObject.SetActive(false);
        m_ScoreText.gameObject.SetActive(false);
        m_HighScoreText.gameObject.SetActive(true);
        m_ResetButton.transform.parent.gameObject.SetActive(true);
    }

    public void Reset()
    {
        m_HighScoreText.gameObject.SetActive(false);
        m_ResetButton.transform.parent.gameObject.SetActive(false);
        //m_FuelBar.gameObject.SetActive(true);
        m_ScoreText.gameObject.SetActive(true);
    }

    void ShowResetButton()
    {
        if (GameManager.Instance.m_GameStarted)
        {
            m_ResetButton.transform.parent.gameObject.SetActive(true);
            m_HighScoreText.gameObject.SetActive(true);
        }
    }

    void HideResetButton()
    {
        m_ResetButton.transform.parent.gameObject.SetActive(false);
        m_HighScoreText.gameObject.SetActive(false);
    }

    void SpeedIncrease()
    {
        sequence = DOTween.Sequence();
        m_SpeedIncreaseText.rectTransform.anchoredPosition.Set(0, -250);
        switch (GameManager.Instance.GetScore())
        {
            case 110:
                m_SpeedIncreaseText.text = "Good luck!";
                break;
            default:
                break;
        }

        sequence.Append(m_SpeedIncreaseText.rectTransform.DOAnchorPosY(0, 0.5f)).Join(m_SpeedIncreaseText.DOFade(1, 0.7f)).Append(m_SpeedIncreaseText.DOFade(0, 1f));
        SoundManager.Instance.PlayPowerUp();
    }

}
