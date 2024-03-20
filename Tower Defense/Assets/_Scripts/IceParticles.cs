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
        private float _fireRateScale;
        private float _rotationSpeed;
        
        private int _idleRotationSpeed;
        
        
        private void Start()
        {
            _rotationSpeed = Random.Range(-1, 1);
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
            FireRateScaleLevelScale();
            if (_IceTowerAnimation.isAttacking == false)
            {
                transform.Rotate(0f, 0f, _idleRotationSpeed);
            }
            
            if (_IceTowerAnimation.isAttacking)
            {
                
                transform.Rotate(0f, 0f, _rotationSpeed * _fireRateScale);
            }
            
        }
        
        private void FireRateScaleLevelScale()
        {
            _fireRateScale = _currentLevelFireRate switch
            {
                0 => 2.5f,
                1 => 3f,
                2 => 3.5f,
                3 => 3.75f,
                4 => 4f,
                5 => 4.25f,
                _ => _fireRateScale
            };
        } 
        
        
        
    }
}
