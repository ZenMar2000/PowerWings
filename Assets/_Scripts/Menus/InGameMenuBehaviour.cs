using UnityEngine;
using UnityEngine.SceneManagement;
using static SharedLogics;

public class InGameMenuBehaviour : MonoBehaviour
{
    public void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonClick()
    {
        TurnOffMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
