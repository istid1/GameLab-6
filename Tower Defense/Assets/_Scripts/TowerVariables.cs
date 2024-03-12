using System;
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

        [HideInInspector] public int _currentFireRateUpgradeLevel = 0;
        
        [HideInInspector] public int _currentRangeUpgradeLevel = 0;
        [HideInInspector] public int _currentDamageUpgradeLevel = 0;

        private bool canAford;
        private TowerFSM _towerFsm;

        [SerializeField] private MoneySystem _moneySystem;

        private void Start()
        {
            _currentDamageUpgradeLevel = 0;
            _currentRangeUpgradeLevel = 0;
            _currentFireRateUpgradeLevel = 0;
            
            GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
            if (gameManagerObject != null)
            {
                _moneySystem = gameManagerObject.GetComponent<MoneySystem>();
            }
        }

        private void Update()
        {
         Debug.Log(_currentDamageUpgradeLevel + "Damage");
         Debug.Log(_currentFireRateUpgradeLevel + "FireRate");
         Debug.Log(_currentRangeUpgradeLevel + "Range");
        }

        public void UpgradeDamage() 
        {
            
                if (_currentDamageUpgradeLevel < 5)
                {
                    _currentDamageUpgradeLevel++;
                    bulletDamage = bulletDamage + _currentDamageUpgradeLevel; ////bulletDamage lvl 5 = 20 (20x the original)
                    //bulletDamage++;   ////bulletDamage lvl 5 = 5 (5x the original) (Linear)
                    //     ^^^ Decide which one we want to use ^^^
                
                    _moneySystem.DeductMoney(50);
                
                }
                if (_currentDamageUpgradeLevel == 5)
                {
                    damageIsUpgraded = true;
                }
        }
        
        public void UpgradeRange() //weaponRange level 5 = 17.5f (Tower gets almost double range) (Linear)
        {
            
                if (_currentRangeUpgradeLevel < 5)
                {
                    _currentRangeUpgradeLevel++;
                    weaponRange = weaponRange + 1.5f;
                
                    _moneySystem.DeductMoney(50);
                }
                if (_currentRangeUpgradeLevel == 5)
                {
                    rangeIsUpgraded = true;
                }
            
          
        }
        public void UpgradeFireRate() //fireRate level 5 = 0.6f (tower shoots almost 2.5x faster)
        {
           
                if (_currentFireRateUpgradeLevel < 5)
                {
                    _currentFireRateUpgradeLevel++;
                    shootRate = shootRate / _currentFireRateUpgradeLevel + 0.5f;
                
                    _moneySystem.DeductMoney(50);
                }
                if (_currentFireRateUpgradeLevel == 5)
                {
                    fireRateIsUpgraded = true;
                }
        }
    }
}
