using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public int bulletDamage = 1;

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
                Destroy(gameObject);
            }
        
    }
}
