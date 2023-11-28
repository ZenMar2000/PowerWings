using System.Collections.Generic;
using UnityEngine;
using static SharedLogics;

public class EnemyGroupHandler : MonoBehaviour
{
    public Vector3 GroupInstantiationBasePosition = new(0, 10, 0);

    public List<EnemySpawnContainer> SpawnContainers;

    private float spawnTimer = 0;
    private Vector3 currentSpawnPosition = Vector3.zero;
    private EnemyGroupHandler spawnedRef = null;
    private bool allEnemiesInstantiated => InstantiatedEnemies == OriginalEnemiesCount;
    private bool followTargetAssigned = false;
    #region Properties
    [SerializeField] private int _enemiesAlive;
    public int EnemiesAlive { get { return _enemiesAlive; } set { _enemiesAlive = value; } }


    private int _originalEnemiesCount = 1;
    public int OriginalEnemiesCount { get { return _originalEnemiesCount; } private set { _originalEnemiesCount = value; } }

    private int _instantiatedEnemies = 0;
    public int InstantiatedEnemies { get { return _instantiatedEnemies; } private set { _instantiatedEnemies = value; } }
    #endregion

    [HideInInspector]
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

        if (EnemiesAlive <= 0 && InstantiatedEnemies >= OriginalEnemiesCount)
        {
            if (GroupManager != null)
                GroupManager.spawnedGroups.Remove(this);

            Destroy(gameObject);
        }

        if (allEnemiesInstantiated && !followTargetAssigned)
        {
            SetFollowedTargets();
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

                    GameObject instantiatedShipSpawner = Instantiate(SpawnContainers[i].EnemyShipSpawner, currentSpawnPosition, Quaternion.identity, transform);
                    EnemySingleSpawnBehaviour shipBehaviour = instantiatedShipSpawner.GetComponent<EnemySingleSpawnBehaviour>();

                    shipBehaviour.HasEnterMove = SpawnContainers[i].HasEnterMove;
                    shipBehaviour.SplineMovementSpeed = SpawnContainers[i].EnterMovementSpeed;
                    shipBehaviour.OnEnterSplineOffsetValue = SpawnContainers[i].EnterOffsetValue;

                    shipBehaviour.GroupHandler = this;
                    shipBehaviour.SplinePathPrefab = SpawnContainers[i].SplinePathPrefab;

                    shipBehaviour.SplineAnimationStartOffset = SpawnContainers[i].SplineAnimationStartOffset;
                    shipBehaviour.StartWithRandomSplinePosition = SpawnContainers[i].StartWithRandomSplinePosition;

                    shipBehaviour.overrideMovementSpeed = SpawnContainers[i].MovementSpeedOverride;


                    //if (SpawnContainers[i].SplineFollowTarget)
                    //{
                    //    if (spawnedRef == null)
                    //    {
                    //        spawnedRef = GroupManager.spawnedGroups.Find(EnemyGroupHandler => EnemyGroupHandler == this);
                    //    }

                    //    if (SpawnContainers[i].FollowTargetIndex < spawnedRef.transform.childCount)
                    //    {
                    //        Transform targetSpawnerTransform = spawnedRef.transform.GetChild(SpawnContainers[i].FollowTargetIndex);
                    //        shipBehaviour.SetTargetToFollow(targetSpawnerTransform.childCount == 2 ? targetSpawnerTransform.GetChild(1) : targetSpawnerTransform.GetChild(0));
                    //    }
                    //}


                    EnemySpawnContainer newContainer = SpawnContainers[i];
                    newContainer.IsSpawned = true;
                    SpawnContainers[i] = newContainer;

                    EnemiesAlive++;
                    InstantiatedEnemies++;

                }
            }
        }
    }

    private void SetFollowedTargets()
    {
        followTargetAssigned = true;
        Transform targetSpawnerTransform;
        Transform currentShipSpawnerTransform;

        for (int i = 0; i < SpawnContainers.Count; i++)
        {
            if (SpawnContainers[i].SplineFollowTarget)
            {
                if (spawnedRef == null)
                {
                    spawnedRef = GroupManager.spawnedGroups.Find(EnemyGroupHandler => EnemyGroupHandler == this);
                }

                targetSpawnerTransform = spawnedRef.transform.GetChild(SpawnContainers[i].FollowTargetIndex);
                currentShipSpawnerTransform = spawnedRef.transform.GetChild(i);
                if (targetSpawnerTransform.childCount == 0)
                {
                    followTargetAssigned = false;
                }
                else if (SpawnContainers[i].FollowTargetIndex < spawnedRef.transform.childCount)
                {
                    currentShipSpawnerTransform.GetComponent<EnemySingleSpawnBehaviour>().SetTargetToFollow(targetSpawnerTransform.childCount == 2 ? targetSpawnerTransform.GetChild(1) : targetSpawnerTransform.GetChild(0));
                }
            }
        }
    }
}
