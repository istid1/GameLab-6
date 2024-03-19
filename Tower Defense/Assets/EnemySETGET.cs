using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySETGET : MonoBehaviour
{
    // Declare and initialize variable
    public bool IsSpeedReduced { get; set; }

    private void Awake()
    {
        IsSpeedReduced = false; 
    }

    public UnityEngine.AI.NavMeshAgent GetNavMeshAgent()
    {
        return this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
}
