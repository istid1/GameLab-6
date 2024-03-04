using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class TowerFSM : FSM
{
    public enum FSMState
    {
        Shoot, DontShoot,
    }

    public FSMState currState;

    //Weapon
    public float weaponRange = 10f;
    public float shootRate;
    private float shootTimer;
    
    


    [SerializeField] private TowerVariables _towerVariables;

    public bool canShoot;

    public List<GameObject> enemyLocList;
    public GameObject enemyParent;
    public GameObject closestEnemy;
    public float distanceToClosestEnemy;

    [SerializeField]
    private GameObject bulletPrefab;
    private int bulletSpeed = 20;

    public int bulletDamage;

    private EnemyParent enemyParentScript;

    protected override void Initialize()
    {
        currState = FSMState.DontShoot;
        enemyParent = GameObject.FindGameObjectWithTag("EnemyParent");
        enemyParentScript = enemyParent.GetComponent<EnemyParent>();
    }

    protected override void FSMUpdate()
    {
        shootRate = _towerVariables.shootRate;
        weaponRange = _towerVariables.weaponRange;
        bulletDamage = _towerVariables.bulletDamage;
        
        switch (currState)
        {
            case FSMState.Shoot: UpdateShootState(); break;
            case FSMState.DontShoot: UpdateDontShootState(); break;
        }

        
        if (enemyParentScript.allEnemies != null)
        {
            FindClosestEnemy();

            if (distanceToClosestEnemy < weaponRange)
            {
                currState = FSMState.Shoot;
            }
            if (distanceToClosestEnemy > weaponRange)
            {
                currState = FSMState.DontShoot;
            }
        }
        else
        {
            currState = FSMState.DontShoot;
        }
    }

    protected override void FSMFixedUpdate()
    {

    }

    protected void UpdateShootState()
    {
        if (enemyParentScript.allEnemies != null)
        {
            
                shootTimer -= Time.deltaTime;

                if (shootTimer <= 0)
                {
                    ShootBullet();
                    shootTimer = shootRate;
                }
            
        }
    }


    protected void UpdateDontShootState()
    {
        shootTimer = 0f;
    }

    private void FindClosestEnemy()
    {
        if (enemyParentScript.allEnemies != null)
        {
            //Clears the zombieList so it doesn't fill up with duplicates
            enemyLocList.Clear();

            //Add the zombie gameobjects in the list
            foreach (Transform child in enemyParent.transform)
            {
                enemyLocList.Add(child.gameObject);
            }


            //Checks which zombie is the closest one, and returns that as the closestZombie
            float closestDistance = float.MaxValue;

            foreach (GameObject zombieLoc in enemyLocList)
            {
                float distance = Vector3.Distance(transform.position, zombieLoc.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = zombieLoc;
                }
            }
            if (closestEnemy != null)
            {
                distanceToClosestEnemy = Vector3.Distance(transform.position, closestEnemy.transform.position);

                if(distanceToClosestEnemy > weaponRange)
                {
                    closestEnemy = null;
                }
            }
        }
    }

    private void ShootBullet()
    {
        if (closestEnemy != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.GetComponent<Projectile>().bulletDamage = bulletDamage;

            Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;

            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);

            bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);

            //Destroy the bullet after it has travelled the weaponRange
            Destroy(bullet, weaponRange / bulletSpeed);
        }
    }

   
    
}

