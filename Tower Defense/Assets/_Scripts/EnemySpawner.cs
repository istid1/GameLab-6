using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] public GameObject[] enemyPrefab;
        
        [SerializeField] private int enemiesSpawnAmount;
        public bool allEnemiesIsSpawned;

        private EnemyParent enemyParent;


        private GameObject stoneParent, iceParent, fireParent, lightningParent, bombParent;
      
        private void Start()
        {
            enemyParent = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
            //SpawnEnemies();

            stoneParent = GameObject.FindGameObjectWithTag("StoneParent");
            iceParent = GameObject.FindGameObjectWithTag("IceParent");
            fireParent = GameObject.FindGameObjectWithTag("FireParent");
            lightningParent = GameObject.FindGameObjectWithTag("LightningParent");
            bombParent = GameObject.FindGameObjectWithTag("BombParent");



        }
        private void Update()
        {
            if (enemiesSpawnAmount == enemyPrefab.Length)
            {
                allEnemiesIsSpawned = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnEnemies();
            }
        }

        private void InstantiateEnemy(int enemyIndex)
        {
            // Bounds checking for index
            if (enemyIndex < 0 || enemyIndex >= enemyPrefab.Length)
            {
                Debug.LogError("Invalid enemy index!");
                return;
            }
        
            GameObject enemyToInstantiate = enemyPrefab[enemyIndex];
            // Provide a position and rotation for the instantiated enemy
            //Instantiate(enemyToInstantiate, new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(150f, 350f)), Quaternion.identity);
            GameObject instantiatedEnemy = Instantiate(enemyToInstantiate, new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(150f, 350f)), Quaternion.identity);
            //instantiatedEnemy.transform.parent = transform;

            if (enemyIndex == 0)
            {
                instantiatedEnemy.transform.parent = stoneParent.transform;
            }

            if (enemyIndex == 1)
            {
                instantiatedEnemy.transform.parent = fireParent.transform;
            }

            if (enemyIndex == 2)
            {
                instantiatedEnemy.transform.parent = iceParent.transform;
            }
            
            if (enemyIndex == 3)
            {
                instantiatedEnemy.transform.parent = lightningParent.transform;
            }
            if (enemyIndex == 4)
            {
                instantiatedEnemy.transform.parent = bombParent.transform;
            }
        }

        private void SpawnEnemies()
        {
            if (enemyParent.allEnemies.Count <= 41)
            {
                for (var i = 0; i < enemiesSpawnAmount; i++)
                {
                    InstantiateEnemy(0);
                    InstantiateEnemy(1);
                    InstantiateEnemy(2);
                }
                enemyParent.AddChildren();
            }
            
        }



       
        
        
    }
}
