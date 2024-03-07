using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        public NavMeshAgent enemyAgent;
        private Transform _target;
        public bool canReachDestination;
        private bool _pathHasBeenMade;
        public NavMeshAgent agent;
        private NavMeshPath _path;
        [SerializeField] private float _movementSpeedAfterWall;
        private GameObject _targetObject;
        
        private int _frameCount = 0;

        private EnemyParent enemyParent;


        private void Awake()
        {
            canReachDestination = true;
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();

            enemyParent = gameObject.GetComponentInParent<EnemyParent>();

            

            //_navMeshAgentQuality = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            _frameCount++;
            
            if (!_pathHasBeenMade)
            {
                _targetObject = new GameObject
                {
                    name = "Enemy Target Position",
                    transform =
                    {
                        position = new Vector3(0, 0, -24)
                    }
                };

                _target = _targetObject.transform;
                _pathHasBeenMade = true;
            }
           
            if (_frameCount >= 10)
            {
                
                ValidatePath();
                enemyAgent.SetDestination(_target.position);
                _frameCount = 0;
            }
            
        }

        private void ValidatePath()
        {
        
            agent.CalculatePath(_target.position, _path);
            switch (_path.status)
            {
                case NavMeshPathStatus.PathComplete:
                   // Debug.Log("Can complete route");
                    canReachDestination = true;
                    break;
                
                case NavMeshPathStatus.PathPartial:
                   // Debug.Log("Can complete halfway");
                    canReachDestination = false;
                    break;
                
                case NavMeshPathStatus.PathInvalid:
                default:
                   
                    canReachDestination = false;
                    break;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("SlowDownWall"))
            {
                agent.speed = _movementSpeedAfterWall;
            }
            if (other.gameObject.CompareTag("EndZone"))
            {
                enemyParent.allEnemies.Remove(this.gameObject);
                DeleteTarget();
                Destroy(this.gameObject);

            }
        }

        public void DeleteTarget()
        {
            Destroy(_targetObject);

        }
    }
}