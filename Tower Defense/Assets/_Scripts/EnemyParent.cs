using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;

public class EnemyParent : MonoBehaviour
{
    //[HideInInspector]
    public List<GameObject> allEnemies;


    public void AddChildren()
    {
        Invoke("AddMyGrandChildren", 2f);
    }
    


    
    
    void AddMyChildren()
    {
        allEnemies.Clear();
        
        foreach (Transform child in transform)
        {
            allEnemies.Add(child.gameObject);
        }
    }
    
    void AddMyGrandChildren()
    {
        allEnemies.Clear();
        
        foreach (Transform child in transform)
        {
            foreach (Transform grandchild in child)
            {
                allEnemies.Add(grandchild.gameObject);
            }
            
            
        }
    }

}