
using _Scripts;

using UnityEngine;

using UnityEngine.UI;



public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private MoneySystem _moneySystem;
    [HideInInspector] public float health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [HideInInspector]
    public string enemyTypeString;

    [HideInInspector] public bool canTakeDamage;

    //public Material stoneEnemyMaterial;
    
    [SerializeField] private Animator _animator;
    
    //[SerializeField] private GameObject _insideModel;
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
    [SerializeField] private GameObject _sliderGameObject;
    

    public enum EnemyType
    {
        Stone,
        Fire,
        Ice,
        Lightning,
        Bomb
    }

    private void Awake()
    {
        _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GetHealth();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //health = 1 + _gameManager.currentRound * 2;
        _maxHealth = health;
        //_slider.maxValue = _maxHealth;
        enemyParentScript = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
        
        enemyMovement = this.gameObject.GetComponent<EnemyMovement>();
        
        enemyFlySpawnerScript = GameObject.FindAnyObjectByType<EnemyFlySpawner>().GetComponent<EnemyFlySpawner>();


        _mR = gameObject.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>();
        
        stoneParent = GameObject.FindGameObjectWithTag("StoneParent");

        CheckEnemyType();

        

    }

    private void DestroyPlz()
    {
        enemyParentScript.allEnemies.Remove(this.gameObject);
        enemyMovement.DeleteTarget();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //_currentHealth = health;
        
        if (health <= 0 && enemyType == EnemyType.Stone)
        {
            _sliderGameObject.SetActive(false);
            
            _animator.SetTrigger("StoneDeath");
            
            Invoke(nameof(DestroyPlz),1f);
            
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
        if (canTakeDamage)      //Makes it so that the flying enemies can't be damaged before they have reached the slowDownWall - Changed to true by SquadLeader
        {
            MoneySystem.Instance.currentMoney++;
            health -= damage;
            UpdateHealthBar(health, _maxHealth);
            //Debug.Log("OW");
        }
    }
    
    void CheckEnemyType()
    {
        if (enemyType == EnemyType.Stone)
        {
            enemyTypeString = "Stone";
            _gm.stoneEnemyHealth = health; // Might not need this anymore
            canTakeDamage = true;
        }
        if (enemyType == EnemyType.Fire)
        {
            enemyTypeString = "Fire";
            canTakeDamage = true;
           
        }
        if (enemyType == EnemyType.Ice)
        {
            enemyTypeString = "Ice";
            canTakeDamage = true;
        }
        if (enemyType == EnemyType.Lightning)
        {
            enemyTypeString = "Lightning";
        }
        if (enemyType == EnemyType.Bomb)
        {
            enemyTypeString = "Bomb";
            canTakeDamage = true;
        }
    }


    private void DeathToStone() // Sets Enemy Typing and triggers animation
    {
       

        if (enemyType == EnemyType.Fire)
        {
            _animator.SetTrigger("FireToStone");
        }
        enemyType = EnemyType.Stone;
        enemyTypeString = "Stone";
        transform.parent = stoneParent.transform;
        
        ;
    }

    void ChangeMeToStone() // triggers animation and updates the Hp bar
    {
        health = _gm.stoneEnemyHealth;
        _maxHealth = _gm.stoneEnemyHealth;
        UpdateHealthBar(health, _maxHealth);
        
        if (enemyType == EnemyType.Ice)
        {
            _animator.SetTrigger("IceToStone");
        }
        
        if (enemyType == EnemyType.Fire)
        {
            _animator.SetTrigger("FireDeath");
        }
        
        Invoke(nameof(DeathToStone), 1f);        

    }


    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _slider.value = currentHealth / maxHealth;
    }


    private void GetHealth() //Get health from GM
    {
        if (enemyType == EnemyType.Stone)
        {
            health = _gm.stoneHealth;
        }
        if (enemyType == EnemyType.Fire)
        {
            health = _gm.fireHealth;
        }
        if (enemyType == EnemyType.Ice)
        {
            health = _gm.iceHealth;
        }
        if (enemyType == EnemyType.Lightning)
        {
            health = _gm.lightningHealth;
        }
        if (enemyType == EnemyType.Bomb)
        {
            health = _gm.bombHealth;
        }
    }
    
}
