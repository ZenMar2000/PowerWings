using ND_VariaBULLET;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SharedLogics;
public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private List<Explosion> explosionEffects = new List<Explosion>();
    [SerializeField] private AudioSource MainMenuMusic;
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
    }

    public void OnEffectsSliderUpdateValue(float value)
    {
        GameInfo.EffectsVolume = value;
        foreach (Explosion explosion in explosionEffects)
        {
            explosion.Attenuation = value;
        }
    }


}
