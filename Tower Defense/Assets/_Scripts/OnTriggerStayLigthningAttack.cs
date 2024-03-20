using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class OnTriggerStayLigthningAttack : MonoBehaviour
    {
        [SerializeField] private LightningAttack _lightningAttack;
        [SerializeField] private TowerVariables _towerVariables;
        private EnemyHealth _enemyHealth;
        private bool _isDamageOverTimeRunning = false;
        private bool _lightningAttackStarted = false; // This is the flag we're adding
        [SerializeField] private int _bulletDamage;
        [SerializeField] private float _shootRate;
        private void Update()
        {
            _bulletDamage = _towerVariables.bulletDamage;
            _shootRate =  _towerVariables.shootRate;
        }
        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Enemy") && _lightningAttack.isInRange && !_lightningAttackStarted) 
            {
                _lightningAttackStarted = true; // Set the flag to true
                _enemyHealth = other.GetComponent<EnemyHealth>();
            
                if (_enemyHealth != null)
                {
                    StartCoroutine(TakeDamageOverTime());
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Enemy") && _enemyHealth != null) 
            {
                StopCoroutine(TakeDamageOverTime());
                _lightningAttackStarted = false; // Reset the flag
            }
        }
        private IEnumerator TakeDamageOverTime()
        {
            const float defaultShootRate = 1f; // Define your default shoot rate value

            while (_enemyHealth != null) // Assuming IsAlive is a property indicating if enemy is alive
            {
                _enemyHealth.TakeDamage(_bulletDamage);

                // Checks if _shootRate is zero, and if so, assigns it a default value
                if (_shootRate == 0) {
                    _shootRate = defaultShootRate;
                }
        
                yield return new WaitForSeconds(1f / _shootRate);
            }
        }
    }
}