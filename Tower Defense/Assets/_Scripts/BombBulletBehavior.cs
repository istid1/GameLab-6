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

        [SerializeField] private GameObject _impactVFXprefab;
        [SerializeField] private GameObject _impactVFXGround;
        [SerializeField] private float dragFactor;

        private Renderer _bomBulletMeshRenderer;
        [SerializeField] private GameObject _bombTrail;
        private Vector3 _lastKnownPosition;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_target == null)
            {
                // When the target is null, use the last known position
                Vector3 direction = (_lastKnownPosition - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;

                // Add a condition to destroy the gameObject, e.g. when it reaches the last known position
                if (Vector3.Distance(transform.position, _lastKnownPosition) < 1f)
                {
                    Destroy(gameObject, 3f);
                }
                return;
            }

            // When the target is not null, store its current position as the last known
            _lastKnownPosition = _target.position;
            if (_impactVFXprefab != null && _target != null)
            {
               
                if(_isCollided)
                    return;

                _bulletDamage = _towerVariables.bulletDamage;
        
                // Check if this script's gameObject is not null before accessing its Transform
                if (_randomDirection)
                {
                    speed = 50;
                }
                else
                {
                    speed = 50;
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
            
            //_impactVFX.Play();
            
            if (other.CompareTag("Enemy"))
            {
                
                Debug.Log(other);
               //_impactVFX.Play();
               
                _impactVFXprefab.SetActive(true);
                
                _enemyHealth = other.GetComponent<EnemyHealth>();
                _bomBulletMeshRenderer = GetComponent<Renderer>();
                _bomBulletMeshRenderer.enabled = false;
                _bombTrail.SetActive(false);
                
                _isCollided = true;
            }
            if (other.CompareTag("GroundDummy"))
            {
                Debug.Log("ground got hit");
                _impactVFXGround.SetActive(true);
                _bombTrail.SetActive(false);
                _bomBulletMeshRenderer.enabled = false;
            }
            
        }
        
    }
}
