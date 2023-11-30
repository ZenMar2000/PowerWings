using ND_VariaBULLET;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SharedLogics;
public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private List<Explosion> explosionEffects = new List<Explosion>();
    [SerializeField] private AudioSource MainMenuMusic;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;

    private void Awake()
    {
        float musicVolume = 1;
        if (PlayerPrefs.HasKey(MusicVolumeString))
        {
            musicVolume = PlayerPrefs.GetFloat(MusicVolumeString);
        }
        else
        {
            PlayerPrefs.SetFloat(MusicVolumeString, musicVolume);
        }

        GameInfo.MusicVolume = musicVolume;
        //MainMenuMusic.volume = musicVolume;

        float fxVolume = 1;
        if (PlayerPrefs.HasKey(MusicVolumeString))
        {
            fxVolume = PlayerPrefs.GetFloat(EffectsVolumeString);
        }
        else
        {
            PlayerPrefs.SetFloat(EffectsVolumeString, fxVolume);
        }

        GameInfo.EffectsVolume = fxVolume;
        foreach (Explosion explosion in explosionEffects)
        {
            explosion.Attenuation = musicVolume;
        }
    }

    public void OnPlayButtonClick()
    {
        TurnOffMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnQuitButtonClick()
    {
        TurnOffMusic();
        Application.Quit();
    }

    public void OnMusicSliderUpdateValue(float value)
    {
        GameInfo.MusicVolume = value;
        MainMenuMusic.volume = value;
        PlayerPrefs.SetFloat(MusicVolumeString, value);
    }

    public void OnEffectsSliderUpdateValue(float value)
    {
        GameInfo.EffectsVolume = value;
        PlayerPrefs.SetFloat(EffectsVolumeString, value);

        foreach (Explosion explosion in explosionEffects)
        {
            explosion.Attenuation = value;
        }
    }
    public void OnResetButtonClick()
    {
        //music
        float musicVolume = 1;
        {
            PlayerPrefs.SetFloat(MusicVolumeString, musicVolume);
        }
        GameInfo.MusicVolume = musicVolume;
        MainMenuMusic.volume = musicVolume;
        musicSlider.value = musicVolume;

        //effects
        float fxVolume = 1;
        {
            PlayerPrefs.SetFloat(EffectsVolumeString, fxVolume);
        }
        GameInfo.EffectsVolume = fxVolume;
        fxSlider.value = fxVolume;
        foreach (Explosion explosion in explosionEffects)
        {
            explosion.Attenuation = musicVolume;
        }

        //score
        PlayerPrefs.SetInt(HighScoreString, 0);
    }
}
