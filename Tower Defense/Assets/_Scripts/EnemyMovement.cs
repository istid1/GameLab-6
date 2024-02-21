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
        
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();

            //_navMeshAgentQuality = GetComponent<NavMeshAgent>();
        }

        void FixedUpdate()
        {
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
            enemyAgent.SetDestination(_target.position);
            
            ValidatePath(); // try to limit the amount the function gets called to 10frames
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
                Destroy(this.gameObject);
                Destroy(_targetObject);
            }
        }
    }
}