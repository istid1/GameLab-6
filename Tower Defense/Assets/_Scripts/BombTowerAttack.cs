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
        // Start is called before the first frame update
        void Start()
        {
            FindEnemies();
        }

        // Update is called once per frame
        void Update()
        {
            
            _weaponRange = _towerVariables.weaponRange;
            _shootRate = _towerVariables.shootRate;
            _weaponDamage = _towerVariables.bulletDamage;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FindEnemies();
            }
            
            
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                ShootBombBullet();
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
                
                    var bullet = Instantiate(_bombBulletPrefab, _spawnPos.transform.position, Quaternion.identity);
                    _shootVFX.Play();
                    bullet.GetComponent<BombBulletBehavior>().SetTargetBomb(_closestEnemy.transform, _towerVariables);
                    
                    _shootTimer = _shootRate;
                }
            
            }
        }
        
        private void FindEnemies()
        {
            _enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        }
    }
}
