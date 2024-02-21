using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;


namespace _Scripts
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] private GameObject transparentTowerPrefab, towerPrefab;

        [Header("Ballista Tower")] 
        [SerializeField] private GameObject transparentBallistaTower;
        [SerializeField] private GameObject ballistaTowerPrefab;

        [Header("Fire Tower")] 
        [SerializeField] private GameObject transparentFireTower; 
        [SerializeField] private GameObject fireTowerPrefab;

        [Header("Ice Tower")] 
        [SerializeField] private GameObject transparentIceTower; 
        [SerializeField] private GameObject iceTowerPrefab;

        [Header("Lightning Tower")] 
        [SerializeField] private GameObject transparentLightningTower;
        [SerializeField] private GameObject lightningTowerPrefab;

        [Header("Bomb Tower")] 
        [SerializeField] private GameObject transparentBombTower;
        [SerializeField] private GameObject bombTowerPrefab;

        [SerializeField] private NavMeshSurface[] navMeshSurfaces;
        [SerializeField] private List<GameObject> placedTower = new List<GameObject>();
        [FormerlySerializedAs("_enemySpawner")] [SerializeField] private EnemySpawner enemySpawner;

        private Camera _mainCamera;
        private bool _userInputActive;
        private const float GridSize = 2.0f;
        private Vector3 _currentRotation = Vector3.zero;
        private Vector3 _transparentTowerLastPos;
        private Vector3 _mouseLastPosition;

        private bool _archerButtonIsPressed, _fireButtonIsPressed, _iceButtonIsPressed, _lightningButtonIsPressed, _bombButtonIsPressed;
        private bool _transparentTowerIsActive;
        private bool _willBlockAgent;
        private bool _runOnce;
        private bool _currentColor;

        private GameObject _currentTransparentTowerInstance;
        private GameObject _instantiatedTransparentTower;
        private GameObject _blockingTower;

        private Renderer _rendererTransparentTower;
        private EnemyMovement _enemyMovement;
        private List<EnemyMovement> _enemyMovements;

        public LayerMask groundLayer;
        public LayerMask towerLayer;
        public LayerMask enemyLayer;
        

        private void Awake()
        {
           BuildNavMeshSurfaces();
           _mainCamera = Camera.main;
        }
        
        // Update is called once per frame
        private void Update()
        {
            CheckColor();
            HasMoved();
            UserInputRotateTower();
            EnemiesCantFinishPath();
            
            if (_userInputActive)
            {
                return;
            }
            if (enemySpawner.allEnemiesIsSpawned && !_runOnce)
            {
                GetEnemyMovementComponents();
                _runOnce = true;
            }
            
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
            {
                HandleArcherTowerSelection(hit);
            }
        }
        
        private void BuildNavMeshSurfaces()
        {
            foreach (var t in navMeshSurfaces)
            {
                t.BuildNavMesh();
            }
        }
        
        private static bool RayCastHitsLayer(Ray ray, LayerMask layer, out RaycastHit hit) //RayCast layers
        {
            return Physics.Raycast(ray, out hit, Mathf.Infinity, layer);
        }
        
        private void CheckColor() //checks if any of the Transparent Tower pieces is RED
        {
            if (_instantiatedTransparentTower == null)
            {
                return;
            }
            var childTowers = _instantiatedTransparentTower.GetComponentsInChildren<Renderer>();
            foreach (var childTower in childTowers)
            {
                if (childTower == null) continue;
                var rend = childTower.GetComponentInChildren<Renderer>();
                if (rend == null || rend.material == null || rend.material.color != Color.red) continue;
                _currentColor = false;
                break;
            } 
        }
        
        private void HandleArcherTowerSelection(RaycastHit hit)
        {
            if (!_archerButtonIsPressed && !_fireButtonIsPressed && !_iceButtonIsPressed &&
                !_lightningButtonIsPressed && !_bombButtonIsPressed) return;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (_currentColor)
                {
                    PlaceTower(hit);
                }
            }
            else if (!_transparentTowerIsActive)
            {
                SpawnTransparentTower(hit);
            }
            if(_transparentTowerIsActive)
            {
                HandleTransparentTower(hit);
            }
        }

        private void PlaceTower(RaycastHit hit)
        {
            if (towerPrefab == null)
            {
                return;
            }
            var newTower = InstantiateTower(towerPrefab, hit);
            if (newTower == null)
            {
                return;
            }
            newTower.transform.localRotation = Quaternion.Euler(_currentRotation);
            placedTower.Add(newTower);
    
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
                _instantiatedTransparentTower = null; // Avoid memory leak
            }
            
            _transparentTowerIsActive = false;
            _blockingTower = newTower;     // Store the reference to the newly placed Tower which might block the path
        }

        private void SpawnTransparentTower(RaycastHit hit)
        {
            _transparentTowerIsActive = true;

            // Compute the object position based on the grid
            var gridPos = SnapToGrid(hit.point, GridSize);

            // Instantiate the object at the correct position
            _instantiatedTransparentTower = Instantiate(transparentTowerPrefab, gridPos, Quaternion.identity);

            // Apply rotation settings to the newly instantiated tower
            _instantiatedTransparentTower.transform.localRotation = Quaternion.Euler(_currentRotation);

            // Access and store the Renderer component for further use
            _rendererTransparentTower = _instantiatedTransparentTower.GetComponent<Renderer>();
        }

        private void HandleTransparentTower(RaycastHit hit)
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            UpdateTransparentTowerPosition(gridPos);
            CheckAndHandleObstacles(hit);
        }

        private void UpdateTransparentTowerPosition(Vector3 gridPos)
        {
            if (_instantiatedTransparentTower != null)
            {
                _instantiatedTransparentTower.transform.position = gridPos;
            }
        }

        private void CheckAndHandleObstacles(RaycastHit hit)
        {
            var tower = hit.transform.gameObject;
            if (tower.CompareTag("Tower"))
            {
                HandleObstruction();
            }
            if (tower.CompareTag("Ground") && !_willBlockAgent)
            {
                _currentColor = true;
            }
        }
        
        private void HandleObstruction()
        {
            _currentColor = false;
        }

        private static GameObject InstantiateTower(GameObject tower, RaycastHit hit)
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            var newTower = Instantiate(tower, gridPos, Quaternion.identity);
            return newTower;
        }
        
        private static Vector3 SnapToGrid(Vector3 rawWorldPos, float gridSize)
        {
            var x = Mathf.RoundToInt(rawWorldPos.x / gridSize);
            var y = Mathf.RoundToInt(rawWorldPos.y / gridSize);
            var z = Mathf.RoundToInt(rawWorldPos.z / gridSize);
            
            return new Vector3(x * gridSize, y * gridSize, z * gridSize);
        }
        
        private void ResetButtonStates()
        {
            _archerButtonIsPressed = false;
            _fireButtonIsPressed = false;
            _iceButtonIsPressed = false;
            _lightningButtonIsPressed = false;
            _bombButtonIsPressed = false;
            _transparentTowerIsActive = false;
        }

        public void ArcherButton()
        {
            ResetButtonStates();
            _archerButtonIsPressed = true;
            
            //destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }

            //Change the prefab to the correct button selection
            transparentTowerPrefab = transparentBallistaTower;
            towerPrefab = ballistaTowerPrefab;

            //spawn the new transparent Tower prefab
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
            {
                SpawnTransparentTower(hit);
            }
            
        }

        public void FireButton()
        {
            ResetButtonStates();
            _fireButtonIsPressed = true;

            //destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            
            //Change the prefab to the correct button selection
            transparentTowerPrefab = transparentFireTower;
            towerPrefab = fireTowerPrefab;
            
            //spawn the new transparent Tower prefab
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
            {
                SpawnTransparentTower(hit);
            }
        }

        public void IceButton()
        {
            ResetButtonStates();
            _iceButtonIsPressed = true;

            //destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            
            //Change the prefab to the correct button selection
            transparentTowerPrefab = transparentIceTower;
            towerPrefab = iceTowerPrefab;
            
            //spawn the new transparent Tower prefab
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
            {
                SpawnTransparentTower(hit);
            }
        }

        public void LightningButton()
        {
            ResetButtonStates();
            _lightningButtonIsPressed = true;

            //destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            
            //Change the prefab to the correct button selection
            transparentTowerPrefab = transparentLightningTower;
            towerPrefab = lightningTowerPrefab;
            
            //spawn the new transparent Tower prefab
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
            {
                SpawnTransparentTower(hit);
            }
        }

        public void BombButton()
        {
            ResetButtonStates();
            _bombButtonIsPressed = true;

            //destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            //Change the prefab to the correct button selection
            transparentTowerPrefab = transparentBombTower;
            towerPrefab = bombTowerPrefab;
            
            //spawn the new transparent Tower prefab
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
            {
                SpawnTransparentTower(hit);
            }
        }
        
        private void UserInputRotateTower()
        {
            if (Input.GetKeyDown(KeyCode.R) && !_userInputActive)
            {
                if (_instantiatedTransparentTower != null)
                {
                    RotateTower(new Vector3(0, 90, 0));
                }
            }
        }
        
        private void RotateTower(Vector3 rotationAngle)
        {
            _userInputActive = true;
            _currentRotation += rotationAngle;
            _instantiatedTransparentTower.transform.DOLocalRotate(rotationAngle, 0.15f, RotateMode.LocalAxisAdd)
                .OnComplete(() => _userInputActive = false);
        }
        
        private static List<EnemyMovement> GetEnemyMovementComponents()
        {
            //Find all active enemy objects in the scene.
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    
            //List to store all the EnemyMovement components.
            var enemyMovements = new List<EnemyMovement>();

            var index = 0;
            for (; index < enemies.Length; index++)
            {
                var enemy = enemies[index];
                //Get the EnemyMovement component and add to the list.
                var enemyMovement = enemy.GetComponent<EnemyMovement>();

                if (enemyMovement != null)
                {
                    enemyMovements.Add(enemyMovement);
                }
            }

            // Return the list of enemy movements.
            return enemyMovements;
        }
        
        
        private void EnemiesCantFinishPath()
        {
            // Get the enemyMovements.
            _enemyMovements = GetEnemyMovementComponents();
            if (_enemyMovements == null) 
            {
              //  Debug.Log("No enemyMovements");
                return;
            }

            if (_enemyMovements.All(enemyMovements => enemyMovements.canReachDestination)) return;
            DestroyBlockTower();
            _willBlockAgent = true;
        }
        
        
        private void DestroyBlockTower()
        {
            if (_blockingTower == null) return;
            placedTower.Remove(_blockingTower);    // remove the Tower from the list
            Destroy(_blockingTower);              // destroy the Tower object
            _blockingTower = null;                // Nullify the reference to avoid deleting the same tower multiple times
            ResetButtonStates();
            Destroy(_instantiatedTransparentTower);
        }
        
        
        private void HasMoved() //Checks if the mouse has moved position to another grid space
        {
            if (_instantiatedTransparentTower == null)
            {
                return;
            }

            if (_mouseLastPosition == _instantiatedTransparentTower.transform.position) return;
            _mouseLastPosition = _instantiatedTransparentTower.transform.position;
            // Debug.Log("Tower Moved");
            _willBlockAgent = false;
        }
        
        
    } 
}