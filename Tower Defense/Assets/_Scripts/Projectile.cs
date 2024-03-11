using System;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public int bulletDamage = 1;

    public int bulletSpeed;

    private GameObject targetEnemy;
    
    private string damageTypeString;
    private EnemyHealth enemyHealth;

    [SerializeField] private DamageType damageType;


    public enum DamageType
    {
        Stone,
        Fire,
        Ice,
        Lightning,
        Bomb
    }


    public void SetTarget(GameObject enemy)
    {
        targetEnemy = enemy;
    }


    private void Start()
    {
        SetDamageType();
    }
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetEnemy != null)
        {
            Vector3 direction = (targetEnemy.transform.position - transform.position).normalized;
            
            //rotate towards the target
            Quaternion lookRotation = quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = lookRotation;
            
            transform.position += direction * bulletSpeed * Time.deltaTime;
        }
        else
        {
            // If enemy is destroyed or missing, destroy the bullet
            Destroy(gameObject);
        }
    }
    
    
    
    
    

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Enemy"))
            {
                
                enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth.enemyTypeString == damageTypeString)
                {
                    
                    enemyHealth.TakeDamage(bulletDamage);
                    Destroy(gameObject);

                }

                else
                {
                    Destroy(gameObject);
                }
                
                //other.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
               // Destroy(gameObject);
            }
        
    }


    private void SetDamageType()
    {
        if (damageType == DamageType.Stone)
        {
            damageTypeString = "Stone";
        }
        if (damageType == DamageType.Fire)
        {
            damageTypeString = "Fire";
        }
        if (damageType == DamageType.Ice)
        {
            damageTypeString = "Ice";
        }
        if (damageType == DamageType.Lightning)
        {
            damageTypeString = "Lightning";
        }
        if (damageType == DamageType.Bomb)
        {
            damageTypeString = "Bomb";
        }
    }
    
    
}
