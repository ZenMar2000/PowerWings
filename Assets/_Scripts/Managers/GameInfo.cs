using Unity.VisualScripting;
using UnityEngine;

public static class GameInfo //: MonoBehaviour
{
    public static readonly long WarningValue = 4503599627370496;

    public static bool IsPlayerAlive = true;

    public static bool HelpScreenVisible = true;

    public static float ThreatLevelUpThreshold = 100;

    public static int ThreatLevel = 1;

    public static bool BossSpawnRequested = false;

    #region Properties
    private static int _threatAccumulator = 0;
    public static int ThreatAccumulator
    {
        get { return _threatAccumulator; }
        private set { _threatAccumulator = value; }
    }

    private static PlayerProjectileEmitterBehaviour _emitterBehaviour;
    public static PlayerProjectileEmitterBehaviour EmitterBehaviour
    {
        get
        {
            return _emitterBehaviour;
        }
        private set
        {
            _emitterBehaviour = value;
        }
    }

    private static GameObject _player;
    public static GameObject Player
    {
        get
        {
            return _player;
        }
        private set
        {
            _player = value;
        }
    }

    private static float _playerScore = 0;
    public static float PlayerScore
    {
        get { return _playerScore; }
        private set { _playerScore = value; }
    }
    #endregion

    public static void Start(GameObject playerShipPrefab, GameManager gameManager)
    {
        Player = Object.Instantiate(playerShipPrefab, new Vector3(0, -10, 0), Quaternion.identity, gameManager.transform);
        EmitterBehaviour = Player.GetComponentInChildren<PlayerProjectileEmitterBehaviour>();
    }

    public static void IncreaseThreat(int value, bool scoreAwarded = true)
    {
        if(scoreAwarded)
            PlayerScore += value * ThreatLevel;

        if (!BossSpawnRequested)
        {
            if (ThreatAccumulator + value >= ThreatLevelUpThreshold)
            {
                BossSpawnRequested = true;
                ThreatAccumulator = 0;

                ThreatLevelUpThreshold *= float.Parse("1," + ThreatLevel);
                ThreatLevel++;
            }
            else if (!BossSpawnRequested)
            {
                ThreatAccumulator += value;
            }
        }
    }

    public static void ResetValues()
    {
        ThreatLevelUpThreshold = 100;
        ThreatLevel = 1;
        BossSpawnRequested = false;
        ThreatAccumulator = 0;
        PlayerScore = 0;
        IsPlayerAlive = true;
    }
}
