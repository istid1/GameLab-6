using UnityEngine;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        //[HideInInspector]
        public float stoneEnemyHealth;
        public Material stoneEnemyMaterial;

        private bool _enemiesIsAlive;
        private bool _startButtonIsPressed = false;
        public int currentRound;
        [SerializeField] private GameObject _startButton;
        private EnemyParent _enemyParent;
        [SerializeField] private EnemySpawner _enemySpawner;

        // Start is called before the first frame update
        void Start()
        {
            currentRound = 0;
            FindEnemyParentInScene();
        }
        // Update is called once per frame
        void Update()
        {
            CheckForEnemies();
            if (!_enemiesIsAlive && _startButtonIsPressed)
            {
                currentRound += 1;
                Debug.Log("Starting round " + currentRound);
                _enemySpawner.SpawnEnemies();
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
                    Debug.Log("There are currently " + _enemyParent.allEnemies.Count + " enemies.");
                }
                else
                {
                    _enemiesIsAlive = false;
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
            _startButtonIsPressed = true;
            currentRound += 1;
            _startButton.SetActive(false);
        }
    }
}
