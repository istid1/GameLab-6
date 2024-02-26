using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    //[HideInInspector]
    public List<GameObject> allEnemies;


    public void AddChildren()
    {
        StartCoroutine(DelayAddChildrenCoroutine());
    }
    


    

    private IEnumerator DelayAddChildrenCoroutine()
    {
        yield return new WaitForSeconds(2f);
        AddMyChildren();
    }
    
    void AddMyChildren()
    {
        allEnemies.Clear();
        
        foreach (Transform child in transform)
        {
            allEnemies.Add(child.gameObject);
        }
    }

}