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
        
        private Dictionary<int, Coroutine> _damageRoutines = new Dictionary<int, Coroutine>();
        
        private void Update()
        {
            _bulletDamage = _towerVariables.bulletDamage;
            _shootRate =  _towerVariables.shootRate;
        }
        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Enemy") && _lightningAttack.isInRange)
            {
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                int enemyID = other.gameObject.GetInstanceID();
             
                if (!_damageRoutines.ContainsKey(enemyID))
                {
                    _damageRoutines[enemyID] = StartCoroutine(TakeDamageOverTime(enemyHealth, enemyID));
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Enemy")) 
            {
                int enemyID = other.gameObject.GetInstanceID();
                if( _damageRoutines.ContainsKey(enemyID) )
                {
                    StopCoroutine(_damageRoutines[enemyID]);
                    _damageRoutines.Remove(enemyID);
                }
            }
        }
        
        
        private void OnEnemyDeath(int enemyID)
        {
            if(_damageRoutines.ContainsKey(enemyID))
            {
                StopCoroutine(_damageRoutines[enemyID]);
                _damageRoutines.Remove(enemyID);
            }
        }
        
        
        private IEnumerator TakeDamageOverTime(EnemyHealth enemy, int enemyID)
        {
            while (enemy != null)
            {
                enemy.TakeDamage(_bulletDamage);
                yield return new WaitForSeconds(1f * _shootRate);
            }

            OnEnemyDeath(enemyID);
        }
    }
}