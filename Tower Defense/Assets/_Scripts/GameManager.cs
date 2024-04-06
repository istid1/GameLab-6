using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //[HideInInspector]
    public float stoneEnemyHealth;

    public Material stoneEnemyMaterial;

    
    [Header("EnemyHealth")] 
    public float stoneHealth;
    public float iceHealth;
    public float fireHealth;
    public float lightningHealth;
    public float bombHealth;
    
    [Header("Speed")] 
    public float speedBeforeWall;
    public float speedAfterWall;
    
    
    //[Header("")] //Space in inspector
    //public float rnd;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
