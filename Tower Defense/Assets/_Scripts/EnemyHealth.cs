using System.Collections;
using System.Collections.Generic;
using _Scripts;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private MoneySystem _moneySystem;
    public float health = 3;
    private float _maxHealth;
    private float _currentHealth;
    [HideInInspector]
    public string enemyTypeString;

    //public Material stoneEnemyMaterial;
    
    
    private EnemyParent enemyParentScript;
    private EnemyMovement enemyMovement;
    private GameObject stoneParent;

    private EnemyFlySpawner enemyFlySpawnerScript;
    
    [SerializeField]
    private SkinnedMeshRenderer _mR;
    [SerializeField]
    private GameManager _gm;

    private GameManager _gameManager;
    
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Slider _slider;
    

    public enum EnemyType
    {
        Stone,
        Fire,
        Ice,
        Lightning,
        Bomb
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        health = 1 + _gameManager.currentRound * 2;
        _maxHealth = health;
        
        enemyParentScript = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
        
        enemyMovement = this.gameObject.GetComponent<EnemyMovement>();
        
        enemyFlySpawnerScript = GameObject.FindAnyObjectByType<EnemyFlySpawner>().GetComponent<EnemyFlySpawner>();


        _mR = gameObject.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>();
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stoneParent = GameObject.FindGameObjectWithTag("StoneParent");

        CheckEnemyType();

        

    }
    

    // Update is called once per frame
    void Update()
    {
        //_currentHealth = health;
        
        if (health <= 0 && enemyType == EnemyType.Stone)
        {
            
            enemyParentScript.allEnemies.Remove(this.gameObject);
            enemyMovement.DeleteTarget();
            
            Destroy(gameObject);
            
        }
        
        if (health <= 0 && enemyType == EnemyType.Lightning)
        {
            
            enemyParentScript.allEnemies.Remove(this.gameObject);
            enemyFlySpawnerScript.hasSquad.Remove(this.gameObject);
            
            Destroy(gameObject);
            
        }
        
        else if (health <= 0 && enemyType != EnemyType.Stone)
        {
            ChangeMeToStone();
        }
        
    }

    public void TakeDamage(float damage)
    {
        
        MoneySystem.Instance.currentMoney++;
        health -= damage;
        UpdateHealthBar(health, _maxHealth);
        //Debug.Log("OW");
    }
    
    void CheckEnemyType()
    {
        if (enemyType == EnemyType.Stone)
        {
            enemyTypeString = "Stone";
            _gm.stoneEnemyHealth = health;
        }
        if (enemyType == EnemyType.Fire)
        {
            enemyTypeString = "Fire";
        }
        if (enemyType == EnemyType.Ice)
        {
            enemyTypeString = "Ice";
        }
        if (enemyType == EnemyType.Lightning)
        {
            enemyTypeString = "Lightning";
        }
        if (enemyType == EnemyType.Bomb)
        {
            enemyTypeString = "Bomb";
        }
    }



    void ChangeMeToStone()
    {
        enemyType = EnemyType.Stone;
        enemyTypeString = "Stone";
        _mR.material = _gm.stoneEnemyMaterial;
        health = _gm.stoneEnemyHealth;
        transform.parent = stoneParent.transform;

    }


    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _slider.value = currentHealth / maxHealth;
    }
    
}
