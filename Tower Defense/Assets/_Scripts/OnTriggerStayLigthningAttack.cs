using System;
using UnityEngine;

namespace _Scripts
{
    public class OnTriggerStayLigthningAttack : MonoBehaviour
    {
        [SerializeField] private LightningAttack _lightningAttack;
        [SerializeField] private TowerVariables _towerVariables;
        private EnemyHealth _enemyHealth;
        private bool _isDamageOverTimeRunning = false;
        [SerializeField] private int _bulletDamage;
        [SerializeField] private float _shootRate;
        private void Update()
        {
            _bulletDamage = _towerVariables.bulletDamage;
            _shootRate = _towerVariables.shootRate;
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Enemy") && _lightningAttack.isInRange) 
            {
                
                _enemyHealth = other.GetComponent<EnemyHealth>();
            
                if (_enemyHealth != null)
                {
                    if (!_isDamageOverTimeRunning) 
                    {
                        InvokeRepeating(nameof(TakeDamageOverTime), _shootRate, _shootRate);
                        _isDamageOverTimeRunning = true;
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Enemy") && _enemyHealth != null) 
            {
                CancelInvoke(nameof(TakeDamageOverTime));
                _isDamageOverTimeRunning = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy") && _enemyHealth != null) 
            {
                
                _enemyHealth.TakeDamage(_bulletDamage);
                _isDamageOverTimeRunning = false;
            }
        }

        private void TakeDamageOverTime()
        {
            if (_enemyHealth != null)// assuming IsAlive is a property indicating if enemy is alive
            {
                _enemyHealth.TakeDamage(_bulletDamage);
            }
            else
            {
                // enemy is dead, stop invoking TakeDamageOverTime
                CancelInvoke(nameof(TakeDamageOverTime));
                _isDamageOverTimeRunning = false;
            }
        }
        
    }
}