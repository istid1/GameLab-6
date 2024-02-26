using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyHealth : MonoBehaviour
{
    public int health = 3;

    [HideInInspector]
    public string enemyType;

    [Header("Enemy Type")]
    public bool stone;
    public bool fire;
    public bool ice;
    public bool lightning;
    public bool bomb;
    

    private EnemyParent enemyParentScript;

    private EnemyMovement enemyMovement;
    
    
    // Start is called before the first frame update
    void Start()
    {
        enemyParentScript = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
        enemyMovement = this.gameObject.GetComponent<EnemyMovement>();

        if (stone)
        {
            enemyType = "Stone";
        }
        if (fire)
        {
            enemyType = "Fire";
        }
        if (ice)
        {
            enemyType = "Ice";
        }
        if (lightning)
        {
            enemyType = "Lightning";
        }
        if (bomb)
        {
            enemyType = "Bomb";
        }
        

        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            enemyParentScript.allEnemies.Remove(this.gameObject);
            enemyMovement.DeleteTarget();
            Destroy(gameObject);
        }
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        //Debug.Log("OW");
    }
}
