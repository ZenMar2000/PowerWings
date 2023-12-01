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
    
    private int bossGroupRepeatPrevention;
    private List<int> recentlySpawnedBossGroups = new List<int>();

    private bool enemyReinforcementCalled = false;

    private void Start()
    {
        groupRepeatPrevention = (int)(NormalSpawns.Count() * 0.5f);
        bossGroupRepeatPrevention = (int)(BossSpawns.Count() * 0.5f);
    }

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
            DestroyAllEnemiesAlive();
            bossSpawned = true;
            int spawnBossAtIndex = RandomnessCheck(true);
            if (recentlySpawnedBossGroups.Count > bossGroupRepeatPrevention)
            {
                recentlySpawnedBossGroups.RemoveAt(0);
            }
            recentlySpawnedBossGroups.Add(spawnBossAtIndex);

            spawnedGroups.Add(Instantiate(BossSpawns[spawnBossAtIndex], transform));
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
    private int RandomnessCheck(bool isBoss = false)
    {

        List<EnemyGroupHandler> listToCheck;
        List<int> recentlySpawnedIndexes;

        if (!isBoss)
        {
            listToCheck = NormalSpawns;
            recentlySpawnedIndexes = recentlySpawnedGroups;
        }
        else
        {
            listToCheck = BossSpawns;
            recentlySpawnedIndexes = recentlySpawnedBossGroups;
        }


        int counter = 0;
        int maxloops = listToCheck.Count * 3;
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, listToCheck.Count);
            counter++;
        }
        while (recentlySpawnedIndexes.Contains(randomIndex) && counter <= maxloops);

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
        spawnedGroups = new List<EnemyGroupHandler>();
    }
}
