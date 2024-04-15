using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        //[HideInInspector]
        public float stoneEnemyHealth;
        //public Material stoneEnemyMaterial;


        [SerializeField] private MoneySystem _moneySystem;
        [SerializeField] private TMP_Text _currRoundText;
        [SerializeField] private TMP_Text _HP;
        private int _frameCount;
        private bool _enemiesIsAlive;
        private bool _hasSpawned;
        private bool _startButtonIsPressed;
        public int currentRound;
        [SerializeField] private GameObject _startButton;
        private EnemyParent _enemyParent;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private EnemyFlySpawner _enemyFlySpawner;

        [SerializeField] private int _playerHealth;

        [SerializeField] private GameObject _gameOverScreen;
        
    
        [Header("EnemyHealth")] 
        public float stoneHealth;
        public float iceHealth;
        public float fireHealth;
        public float lightningHealth;
        public float bombHealth;
    
        [Header("Speed")] 
        public float speedBeforeWall;
        public float speedAfterWall;
        
        
        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1;
            currentRound = -1;
            FindEnemyParentInScene();
        }

        private void Update()
        {
            CheckForEnemies(); // checks how many enemies is in scene

            _currRoundText.text = "Round : " + currentRound;
            _HP.text = "HP : " + _playerHealth;


            if (_playerHealth <= 0)
            {
                Time.timeScale = 0;
                _gameOverScreen.SetActive(true);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            FrameCount();
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            var currentSceneName = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(currentSceneName);
        }
        
        private void FrameCount()
        {
            _frameCount++;
            if (_frameCount >= 150) // 3 second delay before spawning enemies. Makes sure that the bools' has been properly changed.
            {
                
                if (!_enemiesIsAlive && _startButtonIsPressed)
                {
                    CheckAndSpawnEnemies();
                    
                }
                _frameCount = 0;
            } 
        }
        
        private void CheckAndSpawnEnemies()
        {
            if (!_hasSpawned)
            {
                _hasSpawned = true;
                currentRound += 1;
                Debug.Log("Starting round " + currentRound);
                
                
                SpawnEnemiesRoundScale();
                
                _enemiesIsAlive = true;
                
            }
        }

        private void SpawnEnemiesRoundScale()
        {
            _moneySystem.currentMoney += 25 * currentRound;
            for (int i = 0; i < currentRound; i++) // Spawn a stone enemy every round. Amount = currentRound 
            {
                _enemySpawner.SpawnStoneEnemy();
            }
            
            for (int i = 5; i < currentRound; i++) // Spawn Fire Enemy after round 5
            {
                _enemySpawner.SpawnFireEnemy();
            }
            
            for (int i = 10; i < currentRound; i++) // Spawn Ice Enemy after round 10
            {
                _enemySpawner.SpawnIceEnemy();
            }
            
            for (int i = 15; i < currentRound; i++) // Spawn Bomb Enemy after round 15
            {
                _enemySpawner.SpawnBombEnemy();
            }
            for (int i = 20; i < currentRound; i++) // Spawn Flying Enemy after round 15
            {
                _enemyFlySpawner.SpawnFlyingEnemy(1 + currentRound - 20);
            }
            
            
        }
        
        private void FindEnemyParentInScene()
        {
            _enemyParent = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<EnemyParent>();
        }

        private void CheckForEnemies()
        {
            // Ensure that the EnemyParent component has been found
            if (_enemyParent != null)
            {
                // Check if the list is not null and has elements
                if (_enemyParent.allEnemies != null && _enemyParent.allEnemies.Count > 0)
                {
                    _enemiesIsAlive = true;
                    _hasSpawned = true;
                    Debug.Log("There are currently " + _enemyParent.allEnemies.Count + " enemies.");
                }
                else
                {
                    _enemiesIsAlive = false;
                    _hasSpawned = false;
                    
                    Debug.Log("There are currently no enemies in the list.");
                }
            } 
            else 
            {
                Debug.LogError("EnemyParent is not assigned!");
            }
        }
        public void StartGameButton()
        {
            _frameCount = 0;
            _startButtonIsPressed = true;
            currentRound += 1;
            _startButton.SetActive(false);
            
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                _playerHealth--;
            }
        }
        
        
    }
}
