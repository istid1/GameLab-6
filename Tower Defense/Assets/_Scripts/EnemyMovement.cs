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

        void Update()
        {
            if (!_pathHasBeenMade)
            {
                
                
                _targetObject = new GameObject();
                _targetObject.name = "Enemy Target Position";
            
                _targetObject.transform.position = new Vector3(0, 0, -24);
                _target = _targetObject.transform;
                _pathHasBeenMade = true;
            }
            enemyAgent.SetDestination(_target.position);
            
            
            ValidatePath();
        
        }

        public void ValidatePath()
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
                default:
                   // Debug.Log("Cannot reach destination");
                    canReachDestination = false;
                    break;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Collision detected with " + other.gameObject.name);
            if (other.gameObject.CompareTag("SlowDownWall"))
            {
                Debug.Log("I hit a SlowDownWall");
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