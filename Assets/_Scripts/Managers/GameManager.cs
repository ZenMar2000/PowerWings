using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerShipPrefab;
    [SerializeField] private GameObject GameMusicHandler;

    //[SerializeField] private int ThreatLevel;
    //[SerializeField] private int ThreatAccumulator;
    //[SerializeField] private float Threshold;
    //[SerializeField] private float Score;

    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject HelpScreen;

    private void Awake()
    {
        //DontDestroyOnLoad(GameMusicHandler);
        GameInfo.Start(PlayerShipPrefab, this, GameMusicHandler);
        GameInfo.ResetValues();

        HelpScreen.SetActive(GameInfo.HelpScreenVisible);
    }

    private void Update()
    {
        //ThreatLevel = GameInfo.ThreatLevel;
        //ThreatAccumulator = GameInfo.ThreatAccumulator;
        //Threshold = GameInfo.ThreatLevelUpThreshold;
        //Score = GameInfo.PlayerScore;

        if (GameInfo.Player == null && !inGameMenu.gameObject.activeSelf)
        {
            inGameMenu.gameObject.SetActive(true);
        }
    }
}
