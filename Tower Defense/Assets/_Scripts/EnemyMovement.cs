using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent enemyAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        
        enemyAgent.SetDestination(new Vector3(0,0,-18));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
