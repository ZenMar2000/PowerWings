using System.Collections.Generic;
using UnityEngine;
using static SharedLogics;

public class EnemyGroupHandler : MonoBehaviour
{
    public Vector3 GroupInstantiationBasePosition = new(0, 10, 0);
    private float spawnRepeatTimer = 0;

    public List<EnemySpawnContainer> SpawnContainers;
    public int EnemiesAlive => SpawnContainers.Count;
    private int originalEnemiesCout;
    private int instantiatedEnemies;

    private float spawnTimer = 0;
    private Vector3 currentSpawnPosition = Vector3.zero;
    private void Awake()
    {
        EnemySingleSpawnBehaviour singleSpawn;

        originalEnemiesCout = SpawnContainers.Count;
        foreach (EnemySpawnContainer container in SpawnContainers)
        {
            singleSpawn = container.EnemyShipSpawner.GetComponent<EnemySingleSpawnBehaviour>();
            singleSpawn.SplinePathPrefab = container.SplinePathPrefab;
            singleSpawn.overrideMovementSpeed = container.MovementSpeedOverride;
        }
    }

    // Update is called once per frame
    void Update()
    {
        InstantiateGroup();
        if(instantiatedEnemies <= originalEnemiesCout)
        {
            spawnTimer += Time.deltaTime;
        }

        if(EnemiesAlive <= 0)
        {
            Destroy(this);
        }
    }

    private void InstantiateGroup()
    {
        if (instantiatedEnemies < originalEnemiesCout - 1)
        {
            for (int i = 0; i < SpawnContainers.Count; i++)
            {
                if (!SpawnContainers[i].IsSpawned && spawnTimer >= SpawnContainers[i].SpawnDelay)
                {
                    currentSpawnPosition = new Vector3(transform.position.x + SpawnContainers[i].SpawnPositionOffset.x,
                        transform.position.y + SpawnContainers[i].SpawnPositionOffset.y,
                        transform.position.z + SpawnContainers[i].SpawnPositionOffset.z);

                    Instantiate(SpawnContainers[i].EnemyShipSpawner, currentSpawnPosition, Quaternion.identity);
                    
                    EnemySpawnContainer newContainer = SpawnContainers[i];
                    newContainer.IsSpawned = true;
                    SpawnContainers[i] = newContainer;
                    
                    instantiatedEnemies++;
                }
            }
        }
    }

}
