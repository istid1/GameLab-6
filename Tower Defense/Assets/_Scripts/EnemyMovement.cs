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
        private NavMeshPath path;
        private NavMeshAgent _navMeshAgentQuality;


        private void Start()
        {
            GameObject targetObject = new GameObject();
            targetObject.transform.position = new Vector3(0, 0, -18);
            _target = targetObject.transform;
            enemyAgent.SetDestination(_target.position);
            
        }
    }
}
