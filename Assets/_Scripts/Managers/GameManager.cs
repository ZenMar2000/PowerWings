using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerShipPrefab;
    [SerializeField] private int ThreatAccumulator;
    // Start is called before the first frame update
    private void Awake()
    {
        GameInfo.Start(PlayerShipPrefab);
    }
    private void Update()
    {
        ThreatAccumulator = GameInfo.ThreatAccumulator;
    }
}
