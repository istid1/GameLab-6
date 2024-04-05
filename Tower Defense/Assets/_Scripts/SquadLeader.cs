using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader : MonoBehaviour
{
    public float speed = 20f;
    public float speedAfterWall = 5f;
    public List<GameObject> mySquad;

    
    //EnemyFlySpawnerScript is set through instantiate in EnemyFlySpawner;
    public EnemyFlySpawner enemyFlySpawnerScript;
    private Vector3 myWaypoint;

    private GameObject lightningParent;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        lightningParent = GameObject.FindGameObjectWithTag("LightningParent");
        
        myWaypoint = new Vector3(transform.position.x, 4, -28);
        GatherSquad();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, myWaypoint, speed * Time.deltaTime);
    }



    private void GatherSquad()
    {
        foreach (Transform child in lightningParent.transform)
        {
            if (enemyFlySpawnerScript.hasSquad.Contains(child.gameObject) == false)
            {
                mySquad.Add(child.gameObject);
                enemyFlySpawnerScript.hasSquad.Add(child.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FlySlowDownWall"))
        {
            foreach (GameObject child in mySquad)
            {
                child.GetComponent<EnemyFlyMovement>().speed = speedAfterWall;
            }
            
            Destroy((gameObject));
        }
    }
}
