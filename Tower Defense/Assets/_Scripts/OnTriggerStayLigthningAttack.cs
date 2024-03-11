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
        private void Update()
        {
            _bulletDamage = _towerVariables.bulletDamage;
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
                        InvokeRepeating(nameof(TakeDamageOverTime), 1.0f, 1.0f);
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
        private void TakeDamageOverTime()
        {
            _enemyHealth.TakeDamage(_bulletDamage);
        }
        
    }
}