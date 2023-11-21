using UnityEngine;

public static class GameInfo //: MonoBehaviour
{
    public static float ThreatLevelUpThreshold = 100;

    public static int ThreatLevel = 1;

    public static bool BossSpawnRequested = true;

    #region Properties
    private static int _threatAccumulator = 100;
    public static int ThreatAccumulator
    {
        get { return _threatAccumulator; }
        private set { _threatAccumulator = value; }
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

    private static int _playerScore = 0;
    public static int PlayerScore
    {
        get { return _playerScore; }
        private set { _playerScore = value; }
    }
    #endregion

    public static void Start(GameObject playerShipPrefab)
    {
        Player = Object.Instantiate(playerShipPrefab, new Vector3(0, -10, 0), Quaternion.identity);
    }

    public static void IncreaseThreat(int value)
    {
        PlayerScore += (int)(value * float.Parse("1." + (ThreatLevel - 1)));

        if (ThreatAccumulator + value >= ThreatLevelUpThreshold)
        {
            BossSpawnRequested = true;
            ThreatAccumulator = 100;

            ThreatLevelUpThreshold *= float.Parse("1." + ThreatLevel);
            ThreatLevel++;
        }
        else if (!BossSpawnRequested)
        {
            ThreatAccumulator += value;
        }
    }
}
