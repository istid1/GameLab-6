using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class FireBulletBehavoir : MonoBehaviour
    {
        // How long the bullet will go into a random upwards direction
        private const float _RANDOM_DIRECTION_TIME = 1f;

        // The time the bullet started moving
        private float _startTime;

        // Whether the bullet is still in the random direction phase
        private bool _randomDirection;

        // The direction of the bullet during the random phase
        private Vector3 _randomUpwardsDirection;

        public float speed;
        private Transform _target;

        private EnemyHealth _enemyHealth;

        private bool _hasHappened;
        private string damageTypeString;
        private string secondaryDamageString;

        [SerializeField] private TowerVariables _towerVariables;

        [SerializeField] private int _bulletDamage;
        
        [SerializeField] private DamageType damageType;
        [SerializeField] private float dragFactor;
    
        public void SetTargetFire(Transform target, TowerVariables towerVariables) //duplicate SetTarget Function (Projectile)
        {
            
            _target = target;
            // Reset the start time every time a new target is set
            _startTime = Time.time;
            // Bullet will start moving in random upwards direction
            _randomDirection = true;
            // Choose a random upwards direction within some range
            _randomUpwardsDirection = Quaternion.Euler(Random.Range(-45, 45), Random.Range(0, 360), 
                Random.Range(-45, 45)) * Vector3.up;
            _towerVariables = towerVariables;


        }

        private void Start()
        {
            SetDamageType();
            secondaryDamageString = "Stone";
            _hasHappened = false;
        }

        void Update()
        {
            _bulletDamage = _towerVariables.bulletDamage;
            
            if (_randomDirection)
            {
                speed = 15;
            }
            else
            {
                speed = 50;
            }

            if (_target == null)
            {
                Destroy(gameObject);
                return;
            }
            var elapsedTime = Time.time - _startTime;
            // Determine if the bullet should change direction towards the target
            if (_randomDirection && elapsedTime > _RANDOM_DIRECTION_TIME)
            {
                _randomDirection = false;
            }
            Vector3 direction = _randomDirection ?
                _randomUpwardsDirection : (_target.position - transform.position).normalized;

            // Apply drag when bullet is moving upwards
            if (_randomDirection)
            {
            
                direction -= direction * (speed * dragFactor);
            }

            transform.position += direction * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _enemyHealth = other.GetComponent<EnemyHealth>();
                
                    if (_enemyHealth.enemyTypeString == damageTypeString && !_hasHappened)
                    {
                    
                        _enemyHealth.TakeDamage(_bulletDamage);
                        
                        StartCoroutine(DestroyAfterDelay(2f));
                        _hasHappened = true;
                    }
                    if (_enemyHealth.enemyTypeString == secondaryDamageString && !_hasHappened)
                    {
                    
                        _enemyHealth.TakeDamage(_bulletDamage);
                        
                        StartCoroutine(DestroyAfterDelay(2f));
                        _hasHappened = true;
                    }
            }
        }
        IEnumerator DestroyAfterDelay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _enemyHealth.TakeDamage(_bulletDamage);
            _hasHappened = false;
            Destroy(gameObject);
        }
        
        public enum DamageType
        {
            Stone,
            Fire,
            Ice,
            Lightning,
            Bomb
        }
        private void SetDamageType()
        {
            if (damageType == DamageType.Stone)
            {
                damageTypeString = "Stone";
            }
            if (damageType == DamageType.Fire)
            {
                damageTypeString = "Fire";
            }
            if (damageType == DamageType.Ice)
            {
                damageTypeString = "Ice";
            }
            if (damageType == DamageType.Lightning)
            {
                damageTypeString = "Lightning";
            }
            if (damageType == DamageType.Bomb)
            {
                damageTypeString = "Bomb";
            }
        }
        
        
    }
}
