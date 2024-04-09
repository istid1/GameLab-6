using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
namespace _Scripts
{
    public class BombTowerAttack : MonoBehaviour
    {
        [SerializeField] private TowerVariables _towerVariables;
        
        [SerializeField] private List<GameObject> _enemies;
        [SerializeField] private GameObject _bombBulletPrefab;

        [SerializeField] private GameObject _spawnPos;
        
        private float _weaponRange;
        private float _shootRate;
        private int _weaponDamage;
        
        private float _shortestDistance = Mathf.Infinity;
        
        private float _shootTimer;
        [SerializeField] private VisualEffect _shootVFX;
        
        private GameObject _closestEnemy;

        private EnemyParent enemyParentScript;

        private int animSpeedMultiplier;
        private Animator _anim;
        
        private Transform stoneParent, iceParent, fireParent, lightningParent, bombParent;
        
        private GameManager _gameManager;
        private int _currentRound = -1;
        
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            _anim = GetComponent<Animator>();
            
            enemyParentScript = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
            
            stoneParent = GameObject.FindGameObjectWithTag("StoneParent").transform;
            iceParent = GameObject.FindGameObjectWithTag("IceParent").transform;
            fireParent = GameObject.FindGameObjectWithTag("FireParent").transform;
            lightningParent = GameObject.FindGameObjectWithTag("LightningParent").transform;
            bombParent = GameObject.FindGameObjectWithTag("BombParent").transform;
            
            FindEnemies();
            
            
        }

        // Update is called once per frame
        void Update()
        {
            
            _weaponRange = _towerVariables.weaponRange;
            _shootRate = _towerVariables.shootRate;
            _weaponDamage = _towerVariables.bulletDamage;
            
            if (_gameManager.currentRound != _currentRound)
            {
                FindEnemies();
                _currentRound = _gameManager.currentRound;
            
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FindEnemies();
            }
            
            
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                ShootBombBullet();
            }
            
            //FindEnemies();
            
        }

        private void FixedUpdate()
        {
            
            
            if (_towerVariables._currentFireRateUpgradeLevel == 0)
            {
                animSpeedMultiplier = 1;
            }
            else
            {
                animSpeedMultiplier = _towerVariables._currentFireRateUpgradeLevel;

            }
        }


        private GameObject FindClosestEnemy()
        {
            _shortestDistance = Mathf.Infinity;
            foreach (var enemy in _enemies)
            {
                if (enemy == null) continue;

                var distanceToEnemy = (enemy.transform.position - gameObject.transform.position).magnitude;
                if (distanceToEnemy < _shortestDistance)
                {
                    _shortestDistance = distanceToEnemy;
                    _closestEnemy = enemy;
                }
            }
    
            return _closestEnemy;
        }

        
        private void ShootBombBullet()
        {
            if (_closestEnemy != null && _shortestDistance <= _weaponRange)
            {
                _shootTimer -= Time.deltaTime;

                if (_shootTimer <= 0)
                {
                    _anim.SetTrigger("PlayTrigger");
                    _anim.SetFloat("UpgradeMultiplier", animSpeedMultiplier);
                    var bullet = Instantiate(_bombBulletPrefab, _spawnPos.transform.position, Quaternion.identity);
                    _shootVFX.Play();
                    bullet.GetComponent<BombBulletBehavior>().SetTargetBomb(_closestEnemy.transform, _towerVariables);
                    
                    _shootTimer = _shootRate;
                }
            
            }
        }
        
        private void FindEnemies()
        {
            
            ClearList();

            
            foreach (Transform child in stoneParent)
            {
                _enemies.Add(child.gameObject);
            }
            foreach (Transform child in iceParent)
            {
                _enemies.Add(child.gameObject);
            }
            foreach (Transform child in fireParent)
            {
                _enemies.Add(child.gameObject);
            }
            foreach (Transform child in bombParent)
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
