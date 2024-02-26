using UnityEngine;

namespace _Scripts
{
    public class TowerVariables : MonoBehaviour
    {
    
        public bool damageIsUpgraded;
        public bool rangeIsUpgraded;
        public bool fireRateIsUpgraded;

        private TowerFSM _towerFsm;
        


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
            //_towerFsm.shootRate = _towerFsm.shootRate / 10;
            fireRateIsUpgraded = true;
        }

    
    }
}
