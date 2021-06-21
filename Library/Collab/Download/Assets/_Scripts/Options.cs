using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using com.shephertz.app42.paas.sdk.csharp.game;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.EventSystems;

public class Options : MonoBehaviour
{
    [SerializeField] Button m_BackButton;
    [SerializeField] TextMeshProUGUI m_CurrentUserName;
    [SerializeField] Slider m_PlayerSoundSlider;
    [SerializeField] Slider m_MusicSoundSlider;
    [SerializeField] Toggle m_SFXToggle;
    [SerializeField] Toggle m_MusicToggle;
    private bool m_OptionOpened;

    private Vector2 startPosition = Vector3.zero;
    private Vector2 direction;
    public static bool m_IsSelected;
    private bool m_Transition;
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

        m_OptionOpened = false;
        m_Transition = false;
    }

    void ShowOptionPanel()
    {
        m_OptionOpened = true;
        transform.DOLocalMoveX(0, 0.2f);
        m_CurrentUserName.text = App24Leaderboard.m_UserID;
    }
}
