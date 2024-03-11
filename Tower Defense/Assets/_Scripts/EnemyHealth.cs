using System.Collections;
using System.Collections.Generic;
using _Scripts;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class EnemyHealth : MonoBehaviour
{
    public float health = 3;
    
    [HideInInspector]
    public string enemyTypeString;
    
    
    private EnemyParent enemyParentScript;
    private EnemyMovement enemyMovement;

    
    [SerializeField] private EnemyType enemyType;

    

    public enum EnemyType
    {
        Stone,
        Fire,
        Ice,
        Lightning,
        Bomb
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        enemyParentScript = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
        enemyMovement = this.gameObject.GetComponent<EnemyMovement>();


        CheckEnemyType();

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

    public void TakeDamage(float damage)
    {
        health -= damage;
        //Debug.Log("OW");
    }
    
    void CheckEnemyType()
    {
        if (enemyType == EnemyType.Stone)
        {
            enemyTypeString = "Stone";
        }
        if (enemyType == EnemyType.Fire)
        {
            enemyTypeString = "Fire";
        }
        if (enemyType == EnemyType.Ice)
        {
            enemyTypeString = "Ice";
        }
        if (enemyType == EnemyType.Lightning)
        {
            enemyTypeString = "Lightning";
        }
        if (enemyType == EnemyType.Bomb)
        {
            enemyTypeString = "Bomb";
        }
    }
}
