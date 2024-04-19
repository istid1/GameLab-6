using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace _Scripts
{
    public class LightningAttack : MonoBehaviour
    {

        [SerializeField] private TowerVariables _towerVariables;
        

        private float weaponRange;
        [SerializeField] private List<GameObject> _sphereControllers = new List<GameObject>();

        [SerializeField] private VisualEffect _lightingVFX;
        
        public List<GameObject> _enemies;

        [SerializeField] private GameObject _LightningLocalPos;

        public bool isInRange;
        
        private const int NumberOfEnemies = 5;

        [SerializeField]
        private Transform stoneParent, lightningParent;
        [SerializeField]
        private EnemyParent enemyParentScript;

        private GameManager _gameManager;
        private int _currentRound = -1;
        
        private void Awake()
        {
            stoneParent = GameObject.FindGameObjectWithTag("StoneParent").transform;
            lightningParent = GameObject.FindGameObjectWithTag("LightningParent").transform;
            enemyParentScript = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();

            
        }

        private void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            FindEnemies();
        }
        
        private void Update()
        {

            if (_gameManager.tutorial == true)
            {
                weaponRange = 20f;
            }

            if (_gameManager.tutorial == false)
            {
                weaponRange = _towerVariables.weaponRange;
            }
            
            
            
            if (_gameManager.currentRound != _currentRound)
            {
                
                FindEnemies();
                _currentRound = _gameManager.currentRound;
            
            }

            if (_enemies.Count > 0)
            {
                CheckClosestEnemies();
            }
           
            
            if (Input.GetKeyDown(KeyCode.Space))// && enemyParentScript.allEnemies.Count > 0)
            {
                Invoke("FindEnemies", 3f);
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

            bool atLeastOneIsInRange = false;

            for (var i = 0; i < NumberOfEnemies; i++)
            {
                if(i < closestEnemies.Count)
                {
                    _sphereControllers[i].transform.position = closestEnemies[i].transform.position;
                    if (Vector3.Distance(transform.position, closestEnemies[i].transform.position) <= weaponRange)
                    {
                        // Play the Visual Effect as the enemy is in range
                        _lightingVFX.Play();
                        atLeastOneIsInRange = true;
                    }
                }
                else
                {
                    _sphereControllers[i].transform.position = _LightningLocalPos.transform.position;
                }
            }

            isInRange = atLeastOneIsInRange;
        }

        private void FindEnemies()
        {
            
                //Debug.Assert(_sphereControllers.Count == NumberOfEnemies, "Not enough SphereControllers. 5 required.");
                //_enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
                
                ClearList();

                foreach (Transform child in stoneParent)
                {
                    _enemies.Add(child.gameObject);
                }

                foreach (Transform child in lightningParent)
                {
                    _enemies.Add(child.gameObject);
                }
        }
        
        private void ClearList()
        {

            if (_enemies != null)
            {
                if (_enemies.Count > 0)
                {
                    _enemies.Clear();
                }
            }
        
        }
        
        
    }
}
