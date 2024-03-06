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
            
            //transform.LookAt(Vector3(closestEnemy.transform.position.x, transform.position.y, closestEnemy.transform.position.z));
            
            //Vector3 targetPosition = new Vector3(closestEnemy.transform.position.x, transform.position.y, transform.position.z);
            //transform.LookAt(targetPosition);

            //transform.LookAt(closestEnemy.transform);
        }
        

    }
    
    
    // void LookAtTarget()
    // {
    //     Vector3 direction = closestEnemy.transform.position - transform.localPosition;
    //     //direction.y = -180;
    //     direction.z = 90;
    //     //direction.x = 0;
    //     Quaternion rotation = Quaternion.LookRotation(direction);
    //     transform.rotation = rotation;
    // }
    
    void LookAtTarget()
    {
        Vector3 direction = closestEnemy.transform.position - transform.position;
        direction.y = 0; // remove any vertical distance
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, -90); // rotate only along X axis
    }
    
    
}
