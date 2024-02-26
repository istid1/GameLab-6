using System;
using UnityEngine;

namespace _Scripts
{
    public class TowerVariables : MonoBehaviour
    {
    
        public bool damageIsUpgraded;
        public bool rangeIsUpgraded;
        public bool fireRateIsUpgraded;

        public float shootRate =2f;

        private TowerFSM _towerFsm;
        

        private void Start()
        {
           
        }

        public void UpgradeDamage()
        {
            //_towerFsm.bulletDamage = _towerFsm.bulletDamage * 10;
            rangeIsUpgraded = true;
        }
        public void UpgradeRange()
        {
            //_towerFsm.weaponRange = 100f;
            rangeIsUpgraded = true;
        }
        public void UpgradeFireRate()
        {
            shootRate = 0.1f;
            fireRateIsUpgraded = true;
        }

    
    }
}
