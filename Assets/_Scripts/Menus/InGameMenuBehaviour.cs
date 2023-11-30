using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SharedLogics;

public class InGameMenuBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private TMP_Text highscoreNumber;
    public void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonClick()
    {
        TurnOffMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SetHighScore()
    {
        int highscore = 0;
        if (!PlayerPrefs.HasKey(HighScoreString))
        {
            PlayerPrefs.SetInt(HighScoreString, 0);
        }
        else
        {
            highscore = PlayerPrefs.GetInt(HighScoreString);
        }

        if (GameInfo.PlayerScore > highscore)
        {
            PlayerPrefs.SetInt(HighScoreString, (int)GameInfo.PlayerScore);
            highscore = (int)GameInfo.PlayerScore;
            highscoreText.text = "! NEW HIGH SCORE !";
            highscoreText.GetComponent<HighScoreBlink>().enabled = true;
        }

        highscoreNumber.text = highscore.ToString();
    }
}
