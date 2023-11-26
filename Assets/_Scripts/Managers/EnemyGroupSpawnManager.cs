using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGroupSpawnManager : MonoBehaviour
{

    public List<EnemyGroupHandler> NormalSpawns;
    public List<EnemyGroupHandler> BossSpawns;
    [Space(15)]
    public List<EnemyGroupHandler> spawnedGroups = new List<EnemyGroupHandler>();

    [SerializeField] private float spawnTimer = 0;
    private float maxTimerBeforeRespawn = 30;
    private bool bossSpawned = false;

    private int groupRepeatPrevention;
    private List<int> recentlySpawnedGroups = new List<int>();

    private bool enemyReinforcementCalled = false;

    private void Start()
    {
        groupRepeatPrevention = NormalSpawns.Count() / 3;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckGroupStatus();
        RespawnRoutine();
    }

    private void RespawnRoutine()
    {
        CheckIfBossRequested();

        if (spawnedGroups.Count == 0 || spawnTimer >= maxTimerBeforeRespawn)
        {
            if (spawnedGroups.Count < GameInfo.ThreatLevel + 2)
            {
                spawnTimer = 0;
                SpawnRandomGroup();
            }
        }
        else
        {
            if (spawnedGroups.Count < GameInfo.ThreatLevel && !bossSpawned && enemyReinforcementCalled == false)
            {
                spawnTimer = maxTimerBeforeRespawn - 3;
                enemyReinforcementCalled = true;
            }
            spawnTimer += Time.deltaTime;
        }
    }

    private void SpawnRandomGroup()
    {
        int spawnGroupAtIndex = RandomnessCheck();
        enemyReinforcementCalled = false;

        if (recentlySpawnedGroups.Count > groupRepeatPrevention)
        {
            recentlySpawnedGroups.RemoveAt(0);
        }
        recentlySpawnedGroups.Add(spawnGroupAtIndex);

        spawnedGroups.Add(Instantiate(NormalSpawns[spawnGroupAtIndex], transform));
        spawnedGroups.Last().GroupManager = this;

    }

    private void CheckGroupStatus()
    {
        if (!bossSpawned)
        {
            int groupStatus = spawnedGroups.Count;
            foreach (EnemyGroupHandler handler in spawnedGroups)
            {
                if (handler.InstantiatedEnemies <= handler.OriginalEnemiesCount)
                {
                    groupStatus += handler.OriginalEnemiesCount;
                }
                else
                {
                    groupStatus += handler.EnemiesAlive;

                }
            }

            if (groupStatus <= spawnedGroups.Count)
            {
                spawnTimer = maxTimerBeforeRespawn;
            }
        }
    }

    private void CheckIfBossRequested()
    {
        if (GameInfo.BossSpawnRequested && !bossSpawned)
        {//If boss is required to spawn from GameInfo and it's not spawned yet, spawn it.
            bossSpawned = true;
            DestroyAllEnemiesAlive();
            spawnedGroups.Add(Instantiate(BossSpawns[(GameInfo.ThreatLevel - 1) % (BossSpawns.Count)], transform));
            spawnedGroups.Last().GroupManager = this;

            if (GameInfo.ThreatLevel == 1)
            {
                spawnTimer = -maxTimerBeforeRespawn;
            }
            else
            {
                spawnTimer = GetBossTimer();
                if (spawnTimer >= maxTimerBeforeRespawn)
                {
                    SpawnRandomGroup();
                    spawnTimer = 0;
                }
            }

            maxTimerBeforeRespawn -= 0.1f;
        }
        else if (!GameInfo.BossSpawnRequested && bossSpawned)
        {//If boss is defeated (BossShip put GameInfo.BossSpawnRequested to false when defeated), update bossSpawned that it's no longer alive.
            bossSpawned = false;
        }
    }

    private float GetBossTimer()
    {
        return -maxTimerBeforeRespawn * 2 + (maxTimerBeforeRespawn * GameInfo.ThreatLevel * 0.1f);
    }
    private int RandomnessCheck()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, NormalSpawns.Count);
        }
        while (recentlySpawnedGroups.Contains(randomIndex));

        return randomIndex;
    }

    public void DestroyAllEnemiesAlive()
    {
        foreach (EnemyGroupHandler spawnedGroup in spawnedGroups)
        {
            EnemyThreatBehaviour[] enemyShips = spawnedGroup.GetComponentsInChildren<EnemyThreatBehaviour>();
            foreach (EnemyThreatBehaviour singleEnemyShip in enemyShips)
            {
                singleEnemyShip.ForceDestroy(false);
            }

            if(spawnedGroup != null)
            {
                Destroy(spawnedGroup.gameObject);
            }
        }
    }
}
