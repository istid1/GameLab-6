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

        private EnemyParent _enemyParent;
      
        private void Start()
        {
            _enemyParent = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
            SpawnEnemies();
            
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
            instantiatedEnemy.transform.parent = transform;
        }

        private void SpawnEnemies()
        {
            for (var i = 0; i < enemiesSpawnAmount; i++)
            {
                InstantiateEnemy(0);
                InstantiateEnemy(1);
                InstantiateEnemy(2);
            }
            _enemyParent.AddChildren();
        }
    }
}
