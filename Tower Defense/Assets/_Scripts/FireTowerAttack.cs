using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;
using DG.Tweening;

public class FireTowerAttack : MonoBehaviour
{
    [SerializeField] private TowerVariables _towerVariables;
    [SerializeField] private GameObject _fireBulletPrefab;
    private GameObject _closestEnemy;
    private float _shortestDistance = Mathf.Infinity;
    
    [SerializeField]
    private List<GameObject> _enemies;
    
    private float _weaponRange;
    private float _shootRate;
    private int _weaponDamage;

    [SerializeField] private GameObject _bulletSpawnPoint;
    
    private Transform stoneParent, fireParent;
    
    
 

    private float _shootTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        stoneParent = GameObject.FindGameObjectWithTag("StoneParent").transform;
        fireParent = GameObject.FindGameObjectWithTag("FireParent").transform;
        
        FindEnemies();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        _weaponRange = _towerVariables.weaponRange;
        _shootRate = _towerVariables.shootRate;
        _weaponDamage = _towerVariables.bulletDamage;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindEnemies();
        }
        
        
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            ShootFireBullet();
        }
    }


    private GameObject FindClosestEnemy()
    {
        _shortestDistance = Mathf.Infinity;
        foreach (var enemy in _enemies)
        {
            if (enemy == null) continue;

            var distanceToEnemy = (enemy.transform.position - gameObject.transform.position).magnitude;
            if (distanceToEnemy < _shortestDistance)
            {
                _shortestDistance = distanceToEnemy;
                _closestEnemy = enemy;
            }
        }
    
        return _closestEnemy;
    }


    private void ShootFireBullet()
    {
        if (_closestEnemy != null && _shortestDistance <= _weaponRange)
        {
            _shootTimer -= Time.deltaTime;

            if (_shootTimer <= 0)
            {
                
                var bullet = Instantiate(_fireBulletPrefab, _bulletSpawnPoint.transform.position, Quaternion.identity);
                bullet.GetComponent<FireBulletBehavoir>().SetTargetFire(_closestEnemy.transform, _towerVariables);
                
                
                _shootTimer = _shootRate;
            }
            
        }
    }
    
    private void FindEnemies()
    {
        _enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        
        
    }
    
    
    
}
