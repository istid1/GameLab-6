using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyMovement : MonoBehaviour
{

    public Vector3 myWaypoint;
    
    public float speed = 20f;
    //[SerializeField] private float speedAfterWall = 5f;
    
    private EnemyParent enemyParent;

    [HideInInspector] public EnemyFlySpawner enemyFlySpawner;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        enemyParent = gameObject.GetComponentInParent<EnemyParent>();
    }
    
    
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, myWaypoint, speed * Time.deltaTime);
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.CompareTag("FlySlowDownWall"))
        // {
        //     speed = speedAfterWall;
        // }
        
        if (other.gameObject.CompareTag("EndZoneFly"))
        {
            enemyParent.allEnemies.Remove(this.gameObject);
            enemyFlySpawner.hasSquad.Remove(this.gameObject);
            Destroy(this.gameObject);

        }
    }
    
    

}
