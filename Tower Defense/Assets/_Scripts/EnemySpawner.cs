using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class EnemySpawner : MonoBehaviour
    {

        [SerializeField] public GameObject[] enemyPrefab;
        
        [SerializeField] private int enemiesSpawnAmount;

        [SerializeField] private float waitTimeMin;
        [SerializeField] private float waitTimeMax;

        private void Start()
        {
            StartCoroutine(SpawnDelay());
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
            Instantiate(enemyToInstantiate, new Vector3(0f, 0f, Random.Range(125f, 150f)), Quaternion.identity);
        }

        private IEnumerator SpawnDelay()
        {
            for (int i = 0; i < enemiesSpawnAmount; i++)
            {
                InstantiateEnemy(1);
                yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax)); //Spawns enemies at random between max and min wait time
            }
        }
    
    
    }
}

