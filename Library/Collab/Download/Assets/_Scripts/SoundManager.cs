using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private PlayerSound m_PlayerSound;
    [SerializeField] private AudioSource m_Music;
    [SerializeField] private AudioSource m_PlanetExplode;
    [SerializeField] private AudioSource m_PowerUp;

    public static SoundManager Instance;
    private int m_CheckIfMute = 0;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        LoadMusicSettings();
        m_PlayerSound.Initialize();
    }

    private void Start()
    {
        AdManager.Instance.AdBegin += () => PauseAllSounds(true);
        AdManager.Instance.AdComplete += () => PauseAllSounds(false);
    }

    public void SetPlayerVolume(float volume)
    {
        m_PlayerSound.SetVolume(volume);
        m_PlanetExplode.volume = volume;
        m_PowerUp.volume = volume;
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        m_Music.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void ToggleSFX(bool val)
    {
        m_PlayerSound.SetMute(val);
        m_PlanetExplode.mute = val;
        m_PowerUp.mute = val;
    }

    public void ToggleMusic(bool val)
    {
        int value = 0;
        m_Music.mute = val;

        if (val)
            value = 1;
        PlayerPrefs.SetInt("ToggleMusic", value);
    }

    void LoadMusicSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            m_Music.volume = PlayerPrefs.GetFloat("MusicVolume");
        }

        if (PlayerPrefs.HasKey("ToggleMusic"))
        {
            m_CheckIfMute = PlayerPrefs.GetInt("ToggleMusic");

            switch (m_CheckIfMute)
            {
                case 1:
                    m_Music.mute = true;
                    break;
                default:
                    m_Music.mute = false;
                    break;
            }
        }
    }

    public float GetPlayerVolumeValue()
    {
        return m_PlayerSound.GetVolume();
    }

    public bool GetPlayerMuteValue()
    {
        return m_PlayerSound.GetMute();
    }

    public float GetMusicVolume()
    {
        return m_Music.volume;
    }

    public bool GetMusicMute()
    {
        return m_Music.mute;
    }

    public void PauseAllSounds(bool pause)
    {
        switch (pause)
        {
            case true:
                m_Music.Pause();
                m_PlayerSound.Pause(true);
                m_PlanetExplode.Pause();
                m_PowerUp.Pause();
                break;

            case false:
                m_Music.UnPause();
                m_PlanetExplode.UnPause();
                m_PlayerSound.Pause(false);
                m_PowerUp.UnPause();
                break;
        }

    }

    public void PlayPlanetExplosion()
    {
        if (!m_PlanetExplode.isPlaying)
            m_PlanetExplode.PlayOneShot(m_PlanetExplode.clip);
    }

    public void PlayPowerUp()
    {
        if (!m_PowerUp.isPlaying)
            m_PowerUp.PlayOneShot(m_PowerUp.clip);
    }

}
