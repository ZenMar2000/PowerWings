using UnityEngine;

public static class GameManager //: MonoBehaviour
{
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
    public static void Start(GameObject playerShipPrefab)
    {
        Player = UnityEngine.Object.Instantiate(playerShipPrefab, new Vector3(0, -10, 0), Quaternion.identity);
    }
}
