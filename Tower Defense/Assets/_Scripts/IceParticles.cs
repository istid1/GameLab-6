using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class IceParticles : MonoBehaviour
    {
   

        [SerializeField] private TowerVariables _towerVariables;
        [SerializeField] private IceTowerAnimation _IceTowerAnimation;
        
        private int _currentLevelFireRate;
        
        private int _rotationSpeed;
        
        private int _idleRotationSpeed;
        
        
        private void Start()
        {
            _rotationSpeed = Random.Range(-3, 3);
            if (_rotationSpeed == 0)
            {
                _rotationSpeed++;
            }
            _idleRotationSpeed = Random.Range(-1, 1);
            if (_idleRotationSpeed == 0)
            {
                _idleRotationSpeed++;
            }
                
        }
    
        // Update is called once per frame
        private void Update()
        {

            _currentLevelFireRate = _towerVariables._currentFireRateUpgradeLevel;

            if (_IceTowerAnimation.isAttacking == false)
            {
                transform.Rotate(0f, 0f, _idleRotationSpeed);
            }
            
            if (_IceTowerAnimation.isAttacking)
            {
                int rotationSpeed = Math.Abs(_rotationSpeed + _currentLevelFireRate) * -1;
                if (rotationSpeed == 0)
                {
                    rotationSpeed++;
                }
                
                transform.Rotate(0f, 0f, rotationSpeed);
            }
           
            
                
          
            
        }
    }
}
