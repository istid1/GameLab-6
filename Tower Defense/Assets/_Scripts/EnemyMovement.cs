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
                //Find a way to rename the object when it spawns
                GameObject targetObject = new GameObject();
            
                targetObject.transform.position = new Vector3(0, 0, -24);
                _target = targetObject.transform;
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
                    Debug.Log("Can complete route");
                    canReachDestination = true;
                    break;
                case NavMeshPathStatus.PathPartial:
                    Debug.Log("Can complete halfway");
                    canReachDestination = true;
                
                
                    break;
                default:
                    Debug.Log("Cannot reach destination");
                    canReachDestination = false;
                    break;
            }
        }

      
    }
}