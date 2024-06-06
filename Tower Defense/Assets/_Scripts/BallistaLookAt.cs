using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaLookAt : MonoBehaviour
{

    public TowerFSM _towerFsm;

    public GameObject closestEnemy;
    
    
    // Start is called before the first frame update
    void Start()
    {
       _towerFsm = GetComponentInParent<TowerFSM>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_towerFsm.closestEnemy != null)
        {
            closestEnemy = _towerFsm.closestEnemy;
            LookAtTarget();
            
        }
        

    }
    
    
    void LookAtTarget()
    {
        Vector3 direction = closestEnemy.transform.position - transform.position;
        direction.y = 0; // remove any vertical distance
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, -90); // rotate only along X axis
    }
    
    
}
