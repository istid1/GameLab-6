using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyMovement : MonoBehaviour
{

    [HideInInspector] public Vector3 myWaypoint;
    [HideInInspector] public float speed;
    
    [HideInInspector] public EnemyFlySpawner enemyFlySpawner;
    
    private EnemyParent enemyParent;
    private GameManager _gm;


    private void Awake()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyParent = gameObject.GetComponentInParent<EnemyParent>();

        //Grab the Start-speed from gm - The speed is later changed by the SquadLeader
        speed = _gm.speedBeforeWall;
    }
    
    
    void FixedUpdate()
    {
        //Movement
        transform.position = Vector3.MoveTowards(transform.position, myWaypoint, speed * Time.deltaTime);
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("EndZoneFly"))
        {
            
            //Remove the gameobject from the list "HasSquad" & "AllEnemies" - Then destroys the gameobject
            enemyParent.allEnemies.Remove(this.gameObject);   
            enemyFlySpawner.hasSquad.Remove(this.gameObject);
            Destroy(this.gameObject);

        }
    }
    
    

}
