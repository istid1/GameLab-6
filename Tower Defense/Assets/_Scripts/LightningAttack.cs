using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class LightningAttack : MonoBehaviour
    {

        [SerializeField] private TowerController _towerController;
        
        [SerializeField] 
        private List<GameObject> _sphereControllers = new List<GameObject>();

        private List<GameObject> _enemies;

        private const int NumberOfEnemies = 7;

        void Start()
        {
            FindEnemies();
        }
        
        void Update()
        {
            CheckClosestEnemies();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FindEnemies();
            }
        }

        private void CheckClosestEnemies()
        {
            var closestEnemies = new List<GameObject>(_enemies.Count);
            foreach (var enemy in _enemies)
            {
                if (enemy != null)
                {
                    closestEnemies.Add(enemy);
                }
            }

            closestEnemies.Sort((e1, e2) =>
            {
                return Vector3.Distance(transform.position, e1.transform.position)
                    .CompareTo(Vector3.Distance(transform.position, e2.transform.position));
            });

            for (var i = 0; i < NumberOfEnemies; i++)
            {
                if(i < closestEnemies.Count)
                    _sphereControllers[i].transform.position = closestEnemies[i].transform.position;
                else
                    break;
            }
        }

        private void FindEnemies()
        {
            Debug.Assert(_sphereControllers.Count == NumberOfEnemies, "Not enough SphereControllers. 7 required.");
            _enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        }
        
        
    }
}
