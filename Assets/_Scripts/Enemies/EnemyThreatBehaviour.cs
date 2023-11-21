using ND_VariaBULLET;
using UnityEngine;

public class EnemyThreatBehaviour : MonoBehaviour
{
    [SerializeField] private int threatGainOnDefeat = 0;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private ShotCollisionDamage shotCollisionDamage;

    private void OnDestroy()
    {
        GameInfo.IncreaseThreat(threatGainOnDefeat);
        if (isBoss)
        {
            GameInfo.BossSpawnRequested = false;
            if (shotCollisionDamage != null)
            {
                shotCollisionDamage.HP *= float.Parse("1." + (GameInfo.ThreatLevel - 2));
            }
        }
    }
}
