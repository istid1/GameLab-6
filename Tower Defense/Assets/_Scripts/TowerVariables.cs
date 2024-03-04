using UnityEngine;

namespace _Scripts
{
    public class TowerVariables : MonoBehaviour
    {
    
        public bool damageIsUpgraded;
        public bool rangeIsUpgraded;
        public bool fireRateIsUpgraded;

        public float shootRate = 2f;
        public int bulletDamage = 1;
        public float weaponRange = 10f;

        private int _currentFireRateUpgradeLevel = 1;
        private int _currentRangeUpgradeLevel = 1;
        private int _currentDamageUpgradeLevel = 1;
        
        private TowerFSM _towerFsm;
        

        public void UpgradeDamage() 
        {
            if (_currentDamageUpgradeLevel < 6)
            {
                _currentDamageUpgradeLevel++;
                //bulletDamage = bulletDamage + _currentDamageUpgradeLevel; ////bulletDamage lvl 5 = 20 (20x the original)
                bulletDamage++;   ////bulletDamage lvl 5 = 5 (5x the original) (Linear)
                //     ^^^ Decide which one we want to use ^^^
            }
            if (_currentDamageUpgradeLevel == 6)
            {
                damageIsUpgraded = true;
            }
        }
        public void UpgradeRange() //weaponRange level 5 = 17.5f (Tower gets almost double range) (Linear)
        {
            if (_currentRangeUpgradeLevel < 6)
            {
                _currentRangeUpgradeLevel++;
                weaponRange = weaponRange + 1.5f;
            }
            if (_currentRangeUpgradeLevel == 6)
            {
                rangeIsUpgraded = true;
            }
        }
        public void UpgradeFireRate() //fireRate level 5 = 0.6f (tower shoots almost 2.5x faster)
        {
            if (_currentFireRateUpgradeLevel < 6)
            {
                _currentFireRateUpgradeLevel++;
                shootRate = shootRate / _currentFireRateUpgradeLevel + 0.5f;
            }
            if (_currentFireRateUpgradeLevel == 6)
            {
                fireRateIsUpgraded = true;
            }
        }
        
        
        
    }
}
