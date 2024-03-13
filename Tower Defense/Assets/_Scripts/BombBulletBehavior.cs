using UnityEngine;
using UnityEngine.VFX;

namespace _Scripts
{
    public class BombBulletBehavior : MonoBehaviour
    {
    
    
        // How long the bullet will go into a random upwards direction
        private const float _RANDOM_DIRECTION_TIME = 1.5f;

        // The time the bullet started moving
        private float _startTime;

        // Whether the bullet is still in the random direction phase
        private bool _randomDirection;

        // The direction of the bullet during the random phase
        private Vector3 _randomUpwardsDirection;

        public float speed;
        private Transform _target;
        private bool _isCollided = false;
        private EnemyHealth _enemyHealth;

        private bool _hasHappened;
        private string damageTypeString;

        [SerializeField] private TowerVariables _towerVariables;

        [SerializeField] private int _bulletDamage;

        [SerializeField] private VisualEffect _impactVFX;
        [SerializeField] private float dragFactor;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(_isCollided)
                return;

            _bulletDamage = _towerVariables.bulletDamage;
            
            if (_randomDirection)
            {
                speed = 50;
            }
            else
            {
                speed = 50;
            }

            if (_target == null)
            {
                Destroy(gameObject);
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
    
    
        public void SetTargetBomb(Transform target, TowerVariables towerVariables) //duplicate SetTarget Function (Projectile)
        {
            
            _target = target;
            // Reset the start time every time a new target is set
            _startTime = Time.time;
            // Bullet will start moving in random upwards direction
            _randomDirection = true;
            // Set a fixed upwards direction, straight up
            _randomUpwardsDirection = Vector3.up;
            _towerVariables = towerVariables;
        
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
            {
                //_enemyHealth = other.GetComponent<EnemyHealth>();
                _impactVFX.Play();
                _isCollided = true;
            }
        }
    }
}
