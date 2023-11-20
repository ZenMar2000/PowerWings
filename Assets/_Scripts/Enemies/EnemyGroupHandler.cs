using System.Collections.Generic;
using UnityEngine;
using static SharedLogics;

public class EnemyGroupHandler : MonoBehaviour
{
    public Vector3 GroupInstantiationBasePosition = new(0, 10, 0);

    public List<EnemySpawnContainer> SpawnContainers;
    [SerializeField] private int _enemiesAlive;
    public int EnemiesAlive { get { return _enemiesAlive; } set { _enemiesAlive = value; } }

    private int originalEnemiesCount;
    private int instantiatedEnemies = 0;

    private float spawnTimer = 0;
    private Vector3 currentSpawnPosition = Vector3.zero;
    private void Awake()
    {
        originalEnemiesCount = SpawnContainers.Count;
    }

    // Update is called once per frame
    void Update()
    {
        InstantiateGroup();
        if (instantiatedEnemies < originalEnemiesCount)
        {
            spawnTimer += Time.deltaTime;
        }

        if (EnemiesAlive <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void InstantiateGroup()
    {
        if (instantiatedEnemies < originalEnemiesCount)
        {
            for (int i = 0; i < SpawnContainers.Count; i++)
            {
                if (!SpawnContainers[i].IsSpawned && spawnTimer >= SpawnContainers[i].SpawnDelay)
                {
                    currentSpawnPosition = new Vector3(GroupInstantiationBasePosition.x + SpawnContainers[i].SpawnPositionOffset.x,
                        GroupInstantiationBasePosition.y + SpawnContainers[i].SpawnPositionOffset.y,
                        GroupInstantiationBasePosition.z + SpawnContainers[i].SpawnPositionOffset.z);

                    GameObject instantiatedShip = Instantiate(SpawnContainers[i].EnemyShipSpawner, currentSpawnPosition, Quaternion.identity);
                    EnemySingleSpawnBehaviour shipBehaviour = instantiatedShip.GetComponent<EnemySingleSpawnBehaviour>();

                    shipBehaviour.GroupHandler = this;
                    shipBehaviour.SplinePathPrefab = SpawnContainers[i].SplinePathPrefab;
                    EnemySpawnContainer newContainer = SpawnContainers[i];
                    newContainer.IsSpawned = true;
                    SpawnContainers[i] = newContainer;

                    instantiatedEnemies++;
                }
            }
        }
    }

}
