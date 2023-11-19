using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerShipPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        GameManager.Start(PlayerShipPrefab);
    }
}
