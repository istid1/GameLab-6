using System.Collections;
using System.Collections.Generic;
using _Scripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFlySpawner : MonoBehaviour
{

    public GameObject flyingEnemyPrefab;
    public GameObject squadLeaderPrefab;
    public GameObject squadLeaderSpawnPos;
    
    public int amountToSpawn;


    public LayerMask enemyLayer;
    
    [HideInInspector]
    public float enemyRadius = 1.5f;
    
    private float rndX, rndZ;

    private Vector3 targetPos;

    private GameObject lightningParent;

    private EnemyParent _enemyParent;


    public List<GameObject> hasSquad;


    private Vector3 leaderSpawnVector;

    
    // Start is called before the first frame update
    void Start()
    {
        lightningParent = GameObject.FindGameObjectWithTag("LightningParent");
        leaderSpawnVector = squadLeaderSpawnPos.transform.position;
        _enemyParent = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
        
      
    }

    // Update is called once per frame
    void Update()
    {
        
        // if (Input.GetKeyDown(KeyCode.P)) 
        // {
        //     Testing();
        // }
    }

    private void Testing()
    {
        SpawnFlyingEnemy(amountToSpawn);
        
    }
    
    
    
    
    public void SpawnFlyingEnemy(int spawnAmount)
    {
        
        for(int i=0; i < spawnAmount; i++)
        {
            Vector3 myRndPos = Vector3.zero;
            int maxAttempts = 10;

            for (int attempts = 0; attempts < maxAttempts; attempts++)
            {
                RandomPos();
                myRndPos = new Vector3(rndX, 5, rndZ);
                if (Physics.OverlapSphere(myRndPos, enemyRadius, enemyLayer).Length == 0)
                {
                    break; // if free space found, break the loop
                }

                if (attempts >= maxAttempts)
                {
                    Debug.Log("Didn't find an open space");
                    return;
                }
            }
            
            // If a valid position couldn't be found after 'maxAttempts' attempts
            // you may choose to exit this method, throw an error or handle it accordingly

            targetPos = new Vector3(rndX, 5, -28);
            Vector3 direction = targetPos - myRndPos;
            Quaternion rotation = Quaternion.LookRotation(direction);
            var instance = Instantiate(flyingEnemyPrefab, myRndPos, rotation);
            instance.transform.parent = lightningParent.transform;
            var enemyFlyMovement = instance.GetComponent<EnemyFlyMovement>();
            enemyFlyMovement.myWaypoint = targetPos;
            enemyFlyMovement.enemyFlySpawner = this;
        }

        var squadLeader = Instantiate(squadLeaderPrefab, leaderSpawnVector, Quaternion.identity);
        squadLeader.GetComponent<SquadLeader>().enemyFlySpawnerScript = this;

        _enemyParent.AddChildren();
        
    }

    private void RandomPos()
    {
        rndX = Random.Range(-16, 16);
        rndZ = Random.Range(330, 430);
    }
}
