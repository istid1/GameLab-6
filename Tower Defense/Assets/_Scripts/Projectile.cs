using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public int bulletDamage = 1;

    

    [Header("Damage Type")] 
    public bool stone;
    public bool fire;
    public bool ice;
    public bool lightning;
    public bool bomb;

    private string damageType;

    private EnemyHealth enemyHealth;


    private void Start()
    {
        if (stone)
        {
            damageType = "Stone";
        }
        
        if (ice)
        {
            damageType = "Ice";
        }
        
        if (fire)
        {
            damageType = "Fire";
        }
        if (lightning)
        {
            damageType = "Lightning";
        }
        if (bomb)
        {
            damageType = "Bomb";
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Enemy"))
            {
                
                enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth.enemyType == damageType)
                {
                    
                    enemyHealth.TakeDamage(bulletDamage);
                    Destroy(gameObject);

                }
                
                //other.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
               // Destroy(gameObject);
            }
        
    }
}
