using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerShipPrefab;
    [SerializeField] private int ThreatLevel;
    [SerializeField] private int ThreatAccumulator;
    [SerializeField] private float Threshold;
    [SerializeField] private float Score;

    [SerializeField] private GameObject inGameMenu;
    [SerializeField] private GameObject HelpScreen;
    // Start is called before the first frame update
    private void Awake()
    {
        GameInfo.Start(PlayerShipPrefab, this);
        GameInfo.ResetValues();
        HelpScreen.SetActive(GameInfo.HelpScreenVisible);
    }
    private void Update()
    {
        ThreatLevel = GameInfo.ThreatLevel;
        ThreatAccumulator = GameInfo.ThreatAccumulator;
        Threshold = GameInfo.ThreatLevelUpThreshold;
        Score = GameInfo.PlayerScore;

        if(GameInfo.Player == null && !inGameMenu.gameObject.activeSelf)
        {
            inGameMenu.gameObject.SetActive(true);
        }
    }
}
