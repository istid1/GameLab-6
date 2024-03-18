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

    public bool canShoot, hasAnimation;
    public Animator _anim;

    public List<GameObject> enemyLocList;
    public GameObject enemyParent;
    public GameObject closestEnemy;
    public float distanceToClosestEnemy;

    [SerializeField]
    private GameObject bulletPrefab;
    private int bulletSpeed = 25;

    public int bulletDamage;

    private EnemyParent enemyParentScript;

    private Transform bulletSpawnPos;

    private int animSpeedMultiplier;

    [SerializeField] private TowerType towerType;
    public enum TowerType
    {
        Stone, Ice, Fire, Lightning, Bomb,
    }

    private Transform stoneParent, iceParent, fireParent, lightningParent, bombParent;

    public Vector3 bulletVector;
    

    protected override void Initialize()
    {
        currState = FSMState.DontShoot;
        enemyParent = GameObject.FindGameObjectWithTag("EnemyParent");
        enemyParentScript = enemyParent.GetComponent<EnemyParent>();

        stoneParent = GameObject.FindGameObjectWithTag("StoneParent").transform;
        iceParent = GameObject.FindGameObjectWithTag("IceParent").transform;
        fireParent = GameObject.FindGameObjectWithTag("FireParent").transform;
        lightningParent = GameObject.FindGameObjectWithTag("LightningParent").transform;
        bombParent = GameObject.FindGameObjectWithTag("BombParent").transform;
        

        if (hasAnimation)
        {
            _anim = GetComponent<Animator>();

        }

        if (towerType == TowerType.Stone)
        {
            bulletSpawnPos = gameObject.transform.GetChild(1).GetChild(0).GetChild(0);
        }

    }

    protected override void FSMUpdate()
    {
        shootRate = _towerVariables.shootRate;
        weaponRange = _towerVariables.weaponRange;
        bulletDamage = _towerVariables.bulletDamage;

        bulletVector = bulletSpawnPos.position;
        
        switch (currState)
        {
            case FSMState.Shoot: UpdateShootState(); break;
            case FSMState.DontShoot: UpdateDontShootState(); break;
        }

        
        if (enemyParentScript.allEnemies.Count > 0) 
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
        if (_towerVariables._currentFireRateUpgradeLevel == 0)
        {
            animSpeedMultiplier = 1;
        }
        else
        {
            animSpeedMultiplier = _towerVariables._currentFireRateUpgradeLevel;

        }
    }

    protected void UpdateShootState()
    {
        if (enemyParentScript.allEnemies.Count > 0)
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
        if (enemyParentScript.allEnemies.Count > 0) 
        {
            
            
            AddTargets();


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
            if (hasAnimation)
            {
                _anim.SetTrigger("PlayTrigger");
                _anim.SetFloat("UpgradeMultiplier", animSpeedMultiplier);

            }
            
            
            
            //Instantiate projectile
            GameObject bullet = Instantiate(bulletPrefab, bulletVector, Quaternion.identity);
            
            //Calculate direction
            Vector3 directionToEnemy = (closestEnemy.transform.position - bullet.transform.position).normalized;
            
            //Apply rotation
            bullet.transform.rotation = Quaternion.LookRotation(directionToEnemy);
            
            Projectile projectile = bullet.GetComponent<Projectile>();
            
            projectile.bulletDamage = bulletDamage;
            projectile.bulletSpeed = bulletSpeed;
            projectile.SetTarget(closestEnemy);


            

            //Destroy the bullet after it has travelled the weaponRange
            Destroy(bullet, weaponRange / bulletSpeed);
        }
    }

    private void AddStoneTargets()
    {
       
        foreach (Transform child in stoneParent)
        {
            enemyLocList.Add(child.gameObject);
        }
    }

    private void AddTargets()
    {
        if (towerType == TowerType.Stone)
        {
            ClearList();

            AddStoneTargets();
        }

        if (towerType == TowerType.Ice)
        {
            ClearList();

            AddStoneTargets();
            
            foreach (Transform child in iceParent)
            {
                enemyLocList.Add(child.gameObject);
            }
        }

        if (towerType == TowerType.Fire)
        {
            ClearList();

            AddStoneTargets();

            foreach (Transform child in fireParent)
            {
                enemyLocList.Add(child.gameObject);
            }
        }
        if (towerType == TowerType.Lightning)
        {
            ClearList();

            AddStoneTargets();

            foreach (Transform child in lightningParent)
            {
                enemyLocList.Add(child.gameObject);
            }
        }
        if (towerType == TowerType.Bomb)
        {
            ClearList();

            AddStoneTargets();

            foreach (Transform child in bombParent)
            {
                enemyLocList.Add(child.gameObject);
            }
        }
    }


    private void ClearList()
    {

        if (enemyLocList != null)
        {
            if (enemyLocList.Count > 0)
            {
                enemyLocList.Clear();
            }
        }
        
    }
   
    
}

