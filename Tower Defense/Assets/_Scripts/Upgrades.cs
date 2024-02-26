using System.Collections;
using System.Collections.Generic;
using _Scripts;
using TMPro;
using UnityEngine;

public class Upgrades : MonoBehaviour
{

    private TowerController _towerController;
    
    private bool _fireRateIsActive;
    private bool _damageIsActive;
    private bool _rangeIsActive;
         
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FireRate()
    {
        _fireRateIsActive = true;
    }

    private void Damage()
    {
        _damageIsActive = true;
    }

    private void Range()
    {
        _rangeIsActive = true;
    }

    
    
    
}
