using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;

    [Header("List indexes\n" +
        "[0] = Jump_Sound\n" +
        "[1] = Land_Sound\n" +
        "[2] = Death_Sound")]

    public List<AudioClip> m_AudioClips;

    [SerializeField] private float m_Volume = 1;

    private int m_CheckIfMute = 0;

    public void Start()
    {
        if (m_AudioSource == null)
            m_AudioSource = GetComponent<AudioSource>();


    }

    public void Initialize()
    {
        LoadSFXSettings();

        m_AudioSource.volume = m_Volume;
    }

    public void PlayJumpSound()

    {
        if (m_AudioClips != null)
            m_AudioSource.PlayOneShot(m_AudioClips[0]);
    }
    public void PlayLandSound()
    {
        if (m_AudioClips != null)
            m_AudioSource.PlayOneShot(m_AudioClips[1]);
    }

    public void PlayDeathSound()
    {
        if (m_AudioClips != null)
            m_AudioSource.PlayOneShot(m_AudioClips[2]);
    }

    public void SetVolume(float volume)
    {
        m_Volume = volume;
        if (m_AudioSource)
            m_AudioSource.volume = m_Volume;
        PlayerPrefs.SetFloat("SFXVolume", m_Volume);
    }

    public void SetMute(bool val)
    {
        int value = 0;
        m_AudioSource.mute = val;
        if (val)
            value = 1;
        PlayerPrefs.SetInt("ToggleSFX", value);
    }

    void LoadSFXSettings()
    {
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            m_Volume = PlayerPrefs.GetFloat("SFXVolume");
        }

        if (PlayerPrefs.HasKey("ToggleSFX"))
        {
            m_CheckIfMute = PlayerPrefs.GetInt("ToggleSFX");

            switch (m_CheckIfMute)
            {
                case 1:
                    m_AudioSource.mute = true;
                    break;
                default:
                    m_AudioSource.mute = false;
                    break;
            }
        }
    }

    public float GetVolume()
    {
        return m_Volume;
    }

    public bool GetMute()
    {
        return m_AudioSource.mute;
    }

    public void Pause(bool pause)
    {
        if (pause)
            m_AudioSource.Pause();
        else
            m_AudioSource.UnPause();
    }
}
