using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class IceParticleRotation : MonoBehaviour
{

    [SerializeField] private TowerVariables _towerVariables;

    private int _currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        _currentLevel = _towerVariables._currentRangeUpgradeLevel;
        
        transform.Rotate(0f, Random.Range(50f * _currentLevel, 100f * _currentLevel), 0f);
    }
}
