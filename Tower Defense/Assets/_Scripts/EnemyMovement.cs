using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemyAgent.SetDestination(new Vector3(0,0,-18));
        }   
    }
}
