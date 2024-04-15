using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private EnemyHealth _enemyHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_enemyHealth)
        {
            
        }
    }
}
