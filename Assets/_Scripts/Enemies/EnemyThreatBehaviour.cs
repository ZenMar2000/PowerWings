using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThreatBehaviour : MonoBehaviour
{
    [SerializeField] private int threatGainOnDefeat = 0;
    [SerializeField] private bool isBoss =false;

    private void OnDestroy()
    {
        GameInfo.IncreaseThreat(threatGainOnDefeat);
        if (isBoss)
        {
            GameInfo.BossSpawnRequested = false;
        }
    }
}
