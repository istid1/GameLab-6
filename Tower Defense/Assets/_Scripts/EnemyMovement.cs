using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    private Transform _target;
    public bool canReachDestination;
    private bool _pathHasBeenMade = false;
    private NavMeshAgent agent;
    private NavMeshPath path;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    void Update()
    {
        if (!_pathHasBeenMade)
        {
            //Find a way to rename the object when it spawns
            GameObject targetObject = new GameObject();
            
            targetObject.transform.position = new Vector3(0, 0, -18);
            _target = targetObject.transform;
            _pathHasBeenMade = true;
        }
        enemyAgent.SetDestination(_target.position);
            
            if (_target == null)
                return;

            ValidatePath();
        
    }

     void ValidatePath()
    {
        
        agent.CalculatePath(_target.position, path);
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                //Debug.Log("Can complete route");
                canReachDestination = true;
                break;
            case NavMeshPathStatus.PathPartial:
                //Debug.Log("Can complete halfway");
                canReachDestination = false;
                
                break;
            default:
                //Debug.Log("Cannot reach destination");
                canReachDestination = false;
                break;
        }
    }
}
