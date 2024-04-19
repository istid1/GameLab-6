using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class IceTowerAnimation : MonoBehaviour
    {

        [SerializeField] private TowerVariables _towerVariables;
        [SerializeField] private GameObject _projectile;
        
        private static Dictionary<NavMeshAgent, IceTowerAnimation> slowedEnemies = new Dictionary<NavMeshAgent, IceTowerAnimation>();
        private HashSet<NavMeshAgent> enemyComponentsInRange = new HashSet<NavMeshAgent>(); 
        private List<NavMeshAgent> _slowedEnemies = new List<NavMeshAgent>();
        public bool isAttacking;

        [SerializeField] private GameManager _gameManager;
        
        // The radius of the sphere
        private float _radius = 3.0f;
        private bool _enemyInRange;
        
        
        private float _maxDistance = 0f;
        
        private int _currentLevel;  
        
        private int _currentLevelFireRate;
        private float _fireRate;
        
        private float _startScale = 1f;
        private float _idleScale = 1f;
        
        private string secondaryDamageString;
        private string damageTypeString;
        [SerializeField] private DamageType damageType;
        private enum DamageType
        {
            Stone,
            Fire,
            Ice,
            Lightning,
            Bomb
        }


        private void Start()
        {
            _gameManager = _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            SetDamageType();
            secondaryDamageString = "Stone";
        }

        // Update is called once per frame
        private void Update()
        {

            _currentLevel = _towerVariables._currentRangeUpgradeLevel;
            _currentLevelFireRate = _towerVariables._currentFireRateUpgradeLevel;

            if (_gameManager.tutorial == true)
            {
                RadiusScaleWithRangeTUTORIAL();
            }
            if (_gameManager.tutorial == false)
            {
                RadiusScaleWithRange();
            }
            
            FireRateScaleLevelScale();
            
            SphereCast();
            
          
            
        }

        private void UpScale()
        {
            
            isAttacking = true;
            switch (_towerVariables._currentRangeUpgradeLevel)
            {
                case 0:
                    _projectile.transform.DOScale(new Vector3(_startScale + 1f,  _startScale +1f, _startScale + 1f), 0.5f);
                    break;
                case 1:
                    _projectile.transform.DOScale(new Vector3(_startScale + 1.25f, _startScale + 1.25f, _startScale + 1.25f), 0.5f);
                    break;
                case 2:
                    _projectile.transform.DOScale(new Vector3(_startScale + 1.5f, _startScale + 1.5f, _startScale + 1.5f), 0.5f);
                    break;
                case 3:
                    _projectile.transform.DOScale(new Vector3(_startScale + 1.75f, _startScale + 1.75f, _startScale + 1.75f), 0.5f);
                    break;
                case 4:
                    _projectile.transform.DOScale(new Vector3(_startScale + 2f, _startScale + 2f, _startScale + 2f), 0.5f);
                    break;
                case 5:
                    _projectile.transform.DOScale(new Vector3(_startScale + 2.175f, _startScale + 2.175f, _startScale + 2.175f), 0.5f);
                    break;
            }
        }
        private void UpScaleTUTORIAL()
        {
            
            isAttacking = true;
            switch (_towerVariables._currentRangeUpgradeLevel)
            {
                case 0:
                    _projectile.transform.DOScale(new Vector3(_startScale + 5f,  _startScale +5f, _startScale + 5f), 0.5f);
                    break;
                case 1:
                    _projectile.transform.DOScale(new Vector3(_startScale + 4.25f, _startScale + 4.25f, _startScale + 4.25f), 0.5f);
                    break;
                case 2:
                    _projectile.transform.DOScale(new Vector3(_startScale + 1.5f, _startScale + 1.5f, _startScale + 1.5f), 0.5f);
                    break;
                case 3:
                    _projectile.transform.DOScale(new Vector3(_startScale + 1.75f, _startScale + 1.75f, _startScale + 1.75f), 0.5f);
                    break;
                case 4:
                    _projectile.transform.DOScale(new Vector3(_startScale + 2f, _startScale + 2f, _startScale + 2f), 0.5f);
                    break;
                case 5:
                    _projectile.transform.DOScale(new Vector3(_startScale + 2.175f, _startScale + 2.175f, _startScale + 2.175f), 0.5f);
                    break;
            }
        }

        private void DownScale()
        {
            isAttacking = false;
            _projectile.transform.DOScale(new Vector3(_idleScale, _idleScale, _idleScale), 0.5f);
        }
    
        
        private void SphereCast()
        {
            var transform1 = transform;
            var ray = new Ray(transform1.position, transform1.forward);
            RaycastHit[] hits;
            var layerMask = 1 << LayerMask.NameToLayer("Enemy");
            hits = Physics.SphereCastAll(ray, _radius, _maxDistance, layerMask);
            _enemyInRange = false;
            // Declare list to store components
            var enemiesToRestore = new HashSet<NavMeshAgent>();
    
            enemyComponentsInRange.Clear(); // Ensure HashSet is clear before filling
            

            if (hits.Any(hit => hit.collider.gameObject.CompareTag("Enemy")))
            {
                foreach(var hitResult in hits.Where(hit => hit.collider.gameObject.CompareTag("Enemy")))
                {
                    NavMeshAgent component = hitResult.collider.gameObject.GetComponent<NavMeshAgent>();
                    if (component == null) continue;
                    // Add component to the list
                    enemyComponentsInRange.Add(component);

                    if (_slowedEnemies.Contains(component)) continue;
                    component.speed *= 0.9f;
                    _slowedEnemies.Add(component);
                            
                    StartCoroutine(DamageEnemiesOverTime(component, _towerVariables.bulletDamage, _fireRate));
                }
                _enemyInRange = true;
                if (_gameManager.tutorial == true)
                {
                    UpScaleTUTORIAL();
                }

                if (!_gameManager.tutorial)
                {
                    UpScale();
                }
                
            }
    
            // Handle enemies leaving the range.
            foreach (var enemy in _slowedEnemies)
            {
                
                // Check if the enemy or its GameObject has been destroyed
                if (enemy == null || enemy.gameObject == null)
                {
                    enemiesToRestore.Add(enemy);
                    continue;
                }
                
                //Reduce movement speed by /2
                if (enemyComponentsInRange.Contains(enemy)) continue;
                enemy.speed /= 0.9f; // Restore original speed
                enemiesToRestore.Add(enemy);
            }

            foreach (var enemy in enemiesToRestore)
            {
                _slowedEnemies.Remove(enemy);
            }

            // No enemies in range
            if (!_enemyInRange)
            {
                DownScale();
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var transform1 = transform;
            var position = transform1.position;
            var forward = transform1.forward;
            Gizmos.DrawRay(position, forward * _maxDistance);
            Gizmos.DrawWireSphere(position + forward * _maxDistance, _radius);
        }

        private void RadiusScaleWithRange()
        {
            _radius = _currentLevel switch
            {
                0 => 5f,
                1 => 5.5f,
                2 => 6f,
                3 => 6.5f,
                4 => 7f,
                5 => 7.25f,
                _ => _radius
            };
        }
        private void RadiusScaleWithRangeTUTORIAL()
        {
            _radius = _currentLevel switch
            {
                0 => 15f,
                1 => 15f,
                2 => 10f,
                3 => 10f,
                4 => 10f,
                5 => 10f,
                _ => _radius
            };
        }
        
        
        

        private void FireRateScaleLevelScale()
        {
            _fireRate = _currentLevelFireRate switch
            {
                0 => 2f,
                1 => 1.5f,
                2 => 1f,
                3 => 0.5f,
                4 => 0.25f,
                5 => 0.175f,
                _ => _fireRate
            };
        }
        
        
        private IEnumerator DamageEnemiesOverTime(NavMeshAgent enemy, int damage, float damageInterval)
        {
            
            while (_slowedEnemies.Contains(enemy))
            {
                var enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null && enemyHealth.enemyTypeString == damageTypeString)
                {
                    enemyHealth.TakeDamage(damage);
                }
                else if (enemyHealth != null && enemyHealth.enemyTypeString == secondaryDamageString)
                {
                    enemyHealth.TakeDamage(damage);
                }
                yield return new WaitForSeconds(damageInterval);

                // Check if the enemy or its GameObject has been destroyed
                if (enemy == null || enemy.gameObject == null || !enemyComponentsInRange.Contains(enemy))
                {
                    yield break;
                }
            }
        }
        
        
        
        private void SetDamageType()
        {
            damageTypeString = damageType switch
            {
                DamageType.Stone => "Stone",
                DamageType.Fire => "Fire",
                DamageType.Ice => "Ice",
                DamageType.Lightning => "Lightning",
                DamageType.Bomb => "Bomb",
                _ => damageTypeString
            };
        }
        
        
    }
}
