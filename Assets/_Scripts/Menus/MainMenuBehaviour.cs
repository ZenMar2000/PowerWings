using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuBehaviour : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnVolumeChanged(Single single)
    {
        GameInfo.MusicVolume = single;
    }

}
