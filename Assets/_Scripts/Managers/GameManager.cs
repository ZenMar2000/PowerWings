using UnityEngine;
using UnityEngine.InputSystem;
using static SharedLogics;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerShipPrefab;
    [SerializeField] private GameObject GameMusicHandler;

    //[SerializeField] private int ThreatLevel;
    //[SerializeField] private int ThreatAccumulator;
    //[SerializeField] private float Threshold;
    //[SerializeField] private float Score;

    [SerializeField] private GameObject inGameMenu;
    private InGameMenuBehaviour gameMenuBehaviour;

    [SerializeField] private GameObject HelpScreen;

    private void Awake()
    {
        //DontDestroyOnLoad(GameMusicHandler);
        GameInfo.Start(PlayerShipPrefab, this, GameMusicHandler);
        GameInfo.ResetValues();

        HelpScreen.SetActive(GameInfo.HelpScreenVisible);
        gameMenuBehaviour = inGameMenu.GetComponent<InGameMenuBehaviour>();

    }

    private void OnEnable()
    {
        InputManager.InGameMenu.performed += ShowMenu;

        if (!PlayerPrefs.HasKey(HighScoreString))
        { 
            PlayerPrefs.SetInt(HighScoreString, 0);
            PlayerPrefs.Save();
        }

        gameMenuBehaviour.SetHighScore();
    }
    private void OnDisable()
    {
        InputManager.InGameMenu.performed -= ShowMenu;
    }

    private void Update()
    {
        CheckIfGameOver();
    }

    private void ShowMenu(InputAction.CallbackContext context)
    {
        inGameMenu.gameObject.SetActive(!inGameMenu.gameObject.activeSelf);
    }

    private void CheckIfGameOver()
    {
        if (GameInfo.Player == null && !inGameMenu.gameObject.activeSelf)
        {
            if (GameInfo.PlayerScore > PlayerPrefs.GetInt(HighScoreString))
            {
                PlayerPrefs.SetInt(HighScoreString, (int)GameInfo.PlayerScore);
                PlayerPrefs.Save();
                gameMenuBehaviour.SetHighScore();
            }

            inGameMenu.gameObject.SetActive(true);
        }
    }
}
