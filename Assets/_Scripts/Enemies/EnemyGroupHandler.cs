using System.Collections.Generic;
using UnityEngine;
using static SharedLogics;

public class EnemyGroupHandler : MonoBehaviour
{
    public Vector3 GroupInstantiationBasePosition = new(0, 10, 0);

    public List<EnemySpawnContainer> SpawnContainers;

    private float spawnTimer = 0;
    private Vector3 currentSpawnPosition = Vector3.zero;

    #region Properties
    [SerializeField] private int _enemiesAlive;
    public int EnemiesAlive { get { return _enemiesAlive; } set { _enemiesAlive = value; } }


    private int _originalEnemiesCount;
    public int OriginalEnemiesCount { get { return _originalEnemiesCount; } private set { _originalEnemiesCount = value; } }

    private int _instantiatedEnemies = 0;
    public int InstantiatedEnemies { get { return _instantiatedEnemies; } private set { _instantiatedEnemies = value; } }
    #endregion

    public EnemyGroupSpawnManager GroupManager;
    private void Awake()
    {
        OriginalEnemiesCount = SpawnContainers.Count;
    }

    // Update is called once per frame
    void Update()
    {
        InstantiateGroup();
        if (InstantiatedEnemies < OriginalEnemiesCount)
        {
            spawnTimer += Time.deltaTime;
        }

        if (EnemiesAlive <= 0)
        {
            GroupManager.spawnedGroups.Remove(this);
            Destroy(gameObject);
        }
    }

    private void InstantiateGroup()
    {
        if (InstantiatedEnemies < OriginalEnemiesCount)
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
                    shipBehaviour.SplineAnimationStartOffset = SpawnContainers[i].SplineAnimationStartOffset;
                    EnemySpawnContainer newContainer = SpawnContainers[i];
                    newContainer.IsSpawned = true;
                    SpawnContainers[i] = newContainer;

                    EnemiesAlive++;
                    InstantiatedEnemies++;
                }
            }
        }
    }
}
