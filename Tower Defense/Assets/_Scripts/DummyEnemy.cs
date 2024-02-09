using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class DummyEnemy : MonoBehaviour
    {
        public NavMeshAgent enemyAgentDummy;
        private Transform _targetDummy;
        public bool canReachDestinationDummy;
        private bool _pathHasBeenMade;
        public NavMeshAgent agent;
        private NavMeshPath _pathDummy;
        


        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            _pathDummy = new NavMeshPath();

            //_navMeshAgentQuality = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (!_pathHasBeenMade)
            {
                //Find a way to rename the object when it spawns
                GameObject targetObject = new GameObject();
            
                targetObject.transform.position = new Vector3(0, 0, -24);
                _targetDummy = targetObject.transform;
                _pathHasBeenMade = true;
            }
            enemyAgentDummy.SetDestination(_targetDummy.position);
            
            
            ValidatePath();
        
        }

        public void ValidatePath()
        {
        
            agent.CalculatePath(_targetDummy.position, _pathDummy);
            switch (_pathDummy.status)
            {
                case NavMeshPathStatus.PathComplete:
                    Debug.Log("Can complete route");
                    canReachDestinationDummy = true;
                    break;
                case NavMeshPathStatus.PathPartial:
                    Debug.Log("Can complete halfway");
                    canReachDestinationDummy = false;
                
                
                    break;
                default:
                    Debug.Log("Cannot reach destination");
                    canReachDestinationDummy = false;
                    break;
            }
        }
    }
}