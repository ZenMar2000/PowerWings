using ND_VariaBULLET;
using UnityEngine;

public class EnemyThreatBehaviour : MonoBehaviour
{
    [SerializeField] private int threatGainOnDefeat = 0;
    [SerializeField] private bool isBoss = false;

    private bool scoreAwarded = true;
    private ShotCollisionDamage shotCollisionDamage;

    public void ForceDestroy(bool scoreAwardedOnDestroy)
    {
        scoreAwarded = scoreAwardedOnDestroy;
        shotCollisionDamage.HP = 0;
        shotCollisionDamage.CheckIfDefeated();
    }

    private void Start()
    {
        shotCollisionDamage = GetComponentInChildren<ShotCollisionDamage>();
    }

    private void OnDestroy()
    {
        GameInfo.IncreaseThreat(threatGainOnDefeat, scoreAwarded);
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
