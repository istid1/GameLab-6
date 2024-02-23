using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    //[HideInInspector]
    public List<GameObject> allEnemies;


    private float timerDelay = 2f;
    private bool startDelay = true;
    
    // Start is called before the first frame update
    void Start()
    {
        startDelay = true;
    }

    private void Update()
    {
        if (startDelay)
        {
            timerDelay -= Time.deltaTime;
            
            if (timerDelay <= 0)
            {
                AddChildren();

                startDelay = false;
            }
        }
    }


    void AddChildren()
    {
        foreach (Transform child in transform)
        {
            allEnemies.Add(child.gameObject);
        }
    }

}