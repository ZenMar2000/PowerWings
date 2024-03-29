using UnityEngine;

public static class GameInfo
{
    public static float MusicVolume = 1f;

    public static float EffectsVolume = 1f;

    //public static GameObject MusicHandler;

    public static readonly long WarningValue = 4503599627370496;

    public static bool IsPlayerAlive = true;

    public static bool HelpScreenVisible = false;

    public static float ThreatLevelUpThreshold = 100;

    public static int ThreatLevel = 1;

    public static bool BossSpawnRequested = false;

    #region Properties

    //private static bool aaa => MusicHandler != null;
    //public static bool AAA { get { return aaa; }}


    private static int _threatAccumulator = 0;
    public static int ThreatAccumulator
    {
        get { return _threatAccumulator; }
        private set { _threatAccumulator = value; }
    }

    private static PlayerProjectileEmitterBehaviour _playerEmitterBehaviour;
    public static PlayerProjectileEmitterBehaviour PlayerEmitterBehaviour
    {
        get
        {
            return _playerEmitterBehaviour;
        }
        private set
        {
            _playerEmitterBehaviour = value;
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
   
    public static void Start(GameObject playerShipPrefab, GameManager gameManager, GameObject musicHandler)
    {

        Player = Object.Instantiate(playerShipPrefab, new Vector3(0, -10, 0), Quaternion.identity, gameManager.transform);
        PlayerEmitterBehaviour = Player.GetComponentInChildren<PlayerProjectileEmitterBehaviour>();
    }

    public static void IncreaseThreat(int value, bool scoreAwarded = true)
    {
        if (scoreAwarded)
            AddScore(value);

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

    public static void AddScore(float scoreToAdd, bool isBullet = false)
    {
        if (isBullet)
        {
            PlayerScore += scoreToAdd;
        }
        else
        {
            PlayerScore += scoreToAdd * ThreatLevel;
        }
    }
}
