using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Button m_BackButton;
    [SerializeField] TextMeshProUGUI m_CurrentUserName;
    [SerializeField] Slider m_PlayerSoundSlider;
    [SerializeField] Slider m_MusicSoundSlider;
    [SerializeField] Toggle m_SFXToggle;
    [SerializeField] Toggle m_MusicToggle;

    public static bool m_IsSelected;
    private void Awake()
    {
        Menu.Instance.showOptions += ShowOptionPanel;
        m_BackButton.onClick.AddListener(HideOptionPanel);
        m_PlayerSoundSlider.onValueChanged.AddListener(SoundManager.Instance.SetPlayerVolume);
        m_MusicSoundSlider.onValueChanged.AddListener(SoundManager.Instance.SetBackgroundMusicVolume);

        m_SFXToggle.onValueChanged.AddListener((bool value) =>
        {
            m_PlayerSoundSlider.interactable = !value;
            SoundManager.Instance.ToggleSFX(value);
        });

        m_MusicToggle.onValueChanged.AddListener((bool val) =>
        {
            m_MusicSoundSlider.interactable = !val;
            SoundManager.Instance.ToggleMusic(val);
        });
    }

    private void Start()
    {
        m_PlayerSoundSlider.value = SoundManager.Instance.GetPlayerVolumeValue();
        m_MusicSoundSlider.value = SoundManager.Instance.GetMusicVolume();

        m_PlayerSoundSlider.interactable = !SoundManager.Instance.GetPlayerMuteValue();
        m_SFXToggle.isOn = SoundManager.Instance.GetPlayerMuteValue();

        m_MusicSoundSlider.interactable = !SoundManager.Instance.GetMusicMute();
        m_MusicToggle.isOn = SoundManager.Instance.GetMusicMute();


    }

    void HideOptionPanel()
    {
        transform.DOLocalMoveX(1200, 0.2f);
        if (!GameManager.Instance.m_GameStarted)
            Menu.Instance.ToggleMenuButtons(true);
        else
        {
            Menu.Instance.HideOptionScreen();
            Menu.Instance.ToggleMenuButtons(true, false);
        }

    }

    void ShowOptionPanel()
    {
        transform.DOLocalMoveX(0, 0.2f);
        m_CurrentUserName.text = App24Leaderboard.m_UserID;
    }
}
