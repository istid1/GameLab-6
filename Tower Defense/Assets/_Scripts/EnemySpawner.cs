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
      
        private void Start()
        {
            SpawnEnemies();
        }
        private void Update()
        {
            if (enemiesSpawnAmount == enemyPrefab.Length)
            {
                allEnemiesIsSpawned = true;
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
            Instantiate(enemyToInstantiate, new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(150f, 250f)), Quaternion.identity);
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < enemiesSpawnAmount; i++)
            {
                InstantiateEnemy(1);
            }
        }
    }
}
