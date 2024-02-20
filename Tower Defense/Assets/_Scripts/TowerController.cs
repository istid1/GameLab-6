using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using DG.Tweening;


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



        [SerializeField] private CollisionDetection _collisionDetection;
        
        private bool _userInputActive;
        private const float GridSize = 2.0f;
        private bool _archerButtonIsPressed, _fireButtonIsPressed, _iceButtonIsPressed, _lightningButtonIsPressed, _bombButtonIsPressed;
        private GameObject _currentTransparentTowerInstance;
        private GameObject _instantiatedTransparentTower;
       
        private GameObject _blockingTower;
        private bool _willBlockAgent;
        private List<EnemyMovement> _enemyMovements;
        private bool _runOnce;
        private Vector3 _currentRotation = Vector3.zero;
        
        
        private bool _transparentTowerIsActive;
        public bool currentColor;
        private Renderer _rendererTransparentTower;
        [SerializeField] private NavMeshSurface[] navMeshSurfaces;
        
        public LayerMask groundLayer;
        public LayerMask towerLayer;
        public LayerMask enemyLayer;

        private Vector3 _transparentTowerLastPos;
        private Vector3 _mouseLastPosition;
        [SerializeField] private List<GameObject> placedTower = new List<GameObject>();
        private EnemyMovement _enemyMovement;
        [SerializeField]private EnemySpawner _enemySpawner;
        


        private void Awake()
        {
           BuildNavMeshSurfaces();
           
        }
        
        private void BuildNavMeshSurfaces()
        {
            for (int i = 0; i < navMeshSurfaces.Length; i++)
            {
             navMeshSurfaces[i].BuildNavMesh();
             
            }
        }
        

        // Update is called once per frame
        private void Update()
        {
            
            CheckColor();
            if (_userInputActive)
            {
                return;
            }
            if (_enemySpawner.allEnemiesIsSpawned && !_runOnce)
            {
                GetEnemyMovementComponents();
                _runOnce = true;

            }
            
           // Debug.Log("Will it block: " + _willBlockAgent);
            HasMoved();
            
           
            UserInputRotateTower();
          

            if (Camera.main != null)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
                {
                    HandleArcherTowerSelection(hit);
                }
            }
            EnemiesCantFinishPath();
            
        }



        private void CheckColor()
        {
            if (_instantiatedTransparentTower == null)
            {
                Debug.Log("No tower has been instantiated, aborting color check.");
                return;
            }

            var childTowers = _instantiatedTransparentTower.GetComponentsInChildren<Renderer>();
            foreach (var childTower in childTowers)
            {
                if (childTower != null)
                {
                    Renderer rend = childTower.GetComponentInChildren<Renderer>();
                    if (rend != null && rend.material != null && rend.material.color == Color.red)
                    {
                        Debug.Log("ITS WORKING!!!!!!!!!!!");
                        currentColor = false;
                    }
                }
            } 
            
        }

        private bool RaycastHitsLayer(Ray ray, LayerMask layer, out RaycastHit hit)
        {
            return Physics.Raycast(ray, out hit, Mathf.Infinity, layer);
        }

        private void HandleArcherTowerSelection(RaycastHit hit)
        {
            if (_archerButtonIsPressed || _fireButtonIsPressed || _iceButtonIsPressed || _lightningButtonIsPressed || _bombButtonIsPressed)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (currentColor)
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
        }

        private void PlaceTower(RaycastHit hit)
        {
            
            if (towerPrefab == null)
            {
                
                return;
            }
            
            GameObject newTower = InstantiateTower(towerPrefab, hit);
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
            
            _instantiatedTransparentTower = InstantiateTransparentTower(transparentTowerPrefab, hit);
            _instantiatedTransparentTower.transform.localRotation = Quaternion.Euler(_currentRotation);
            _rendererTransparentTower = _instantiatedTransparentTower.GetComponent<Renderer>();
            _transparentTowerIsActive = true;
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
            //Debug.DrawRay(hit.point, -mouseRay.direction * 10, Color.yellow);
           
            
            GameObject tower = hit.transform.gameObject;
            if (tower.CompareTag("Tower"))
            {
                HandleObstruction();
            }
            if (tower.CompareTag("Ground") && !_willBlockAgent)
            {
                currentColor = true;
            }
        }

     

        private void HandleObstruction()
        {
            currentColor = false;
        }

        private GameObject InstantiateTower(GameObject tower, RaycastHit hit)
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            GameObject newTower = Instantiate(tower, gridPos, Quaternion.identity);
            return newTower;
        }
        
        
        //This function seem "REDUNDANT" xD  //Look for ways to remove
        private GameObject InstantiateTransparentTower(GameObject transparentTower, RaycastHit hit)
        {
            _transparentTowerIsActive = true;

            var gridPos = SnapToGrid(hit.point, GridSize);
            GameObject towerInstance = Instantiate(transparentTower, gridPos, Quaternion.identity);
            _rendererTransparentTower = towerInstance.GetComponent<Renderer>();
            
            return towerInstance;

        }
   
        private Vector3 SnapToGrid(Vector3 rawWorldPos, float gridSize)
        {
            int x = Mathf.RoundToInt(rawWorldPos.x / gridSize);
            int y = Mathf.RoundToInt(rawWorldPos.y / gridSize);
            int z = Mathf.RoundToInt(rawWorldPos.z / gridSize);

            return new Vector3(x * gridSize, y * gridSize, z * gridSize);
        }

        // public void PlayerButtonInputArcher()
        // {
        //     _archerButtonIsPressed = true;
        // }

       
        
        
        private void ResetButtonStates()
        {
            //noButtonIsPressed = false;

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
            
    
            // destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }

            // reset transparentTowerPrefab for spawning new one on click
            transparentTowerPrefab = transparentBallistaTower;
            towerPrefab = ballistaTowerPrefab;

            // spawn the new transparent tower immediately
            if (Camera.main != null)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
                {
                    SpawnTransparentTower(hit);
                }
            }
        }

        public void FireButton()
        {
            ResetButtonStates();
            _fireButtonIsPressed = true;

            // destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            // reset transparentTowerPrefab for spawning new one on click
            transparentTowerPrefab = transparentFireTower;
            towerPrefab = fireTowerPrefab;
            // spawn the new transparent tower immediately
            if (Camera.main != null)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
                {
                    SpawnTransparentTower(hit);
                }
            }
        }

        public void IceButton()
        {
            ResetButtonStates();
            _iceButtonIsPressed = true;

            // destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            // reset transparentTowerPrefab for spawning new one on click
            transparentTowerPrefab = transparentIceTower;
            towerPrefab = iceTowerPrefab;
            // spawn the new transparent tower immediately
            if (Camera.main != null)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
                {
                    SpawnTransparentTower(hit);
                }
            }
        }

        public void LightningButton()
        {
            ResetButtonStates();
            _lightningButtonIsPressed = true;

            // destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            // reset transparentTowerPrefab for spawning new one on click
            transparentTowerPrefab = transparentLightningTower;
            towerPrefab = lightningTowerPrefab;
            // spawn the new transparent tower immediately
            if (Camera.main != null)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
                {
                    SpawnTransparentTower(hit);
                }
            }
        }

        public void BombButton()
        {
            ResetButtonStates();
            _bombButtonIsPressed = true;

            // destroy the old tower if exists
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
            }
            // reset transparentTowerPrefab for spawning new one on click
            transparentTowerPrefab = transparentBombTower;
            towerPrefab = bombTowerPrefab;
            // spawn the new transparent tower immediately
            if (Camera.main != null)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
                {
                    SpawnTransparentTower(hit);
                }
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
        
        
       
        
        private List<EnemyMovement> GetEnemyMovementComponents()
        {
            // Find all active enemy objects in the scene.
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    
            // List to store all the EnemyMovement components.
            List<EnemyMovement> enemyMovements = new List<EnemyMovement>();

            foreach (GameObject enemy in enemies)
            {
                // Get the EnemyMovement component and add to the list.
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        
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

            foreach(var enemyMovements in _enemyMovements)
            {
                if(!enemyMovements.canReachDestination)
                {
                    //Debug.Log("Enemy can't reach destination");   
                    DestroyBlockTower();
                    _willBlockAgent = true;
                    return;
                }
            }
        }
        
        
        private void DestroyBlockTower()
        {
            if (_blockingTower != null)
            {
                placedTower.Remove(_blockingTower);    // remove from the list
                Destroy(_blockingTower);              // destroy the tower object
                _blockingTower = null;                // Nullify the reference to avoid deleting the same tower multiple times
                ResetButtonStates();
                Destroy(_instantiatedTransparentTower);
                
            }
        }
        
        
        private bool HasMoved() //Checks if the mouse has moved position to another grid space
        {
            if (_instantiatedTransparentTower == null)
            {
                //Debug.Log("Tower is null");
                return false;
            }
            if (_mouseLastPosition != _instantiatedTransparentTower.transform.position)
            {
                _mouseLastPosition = _instantiatedTransparentTower.transform.position;
               // Debug.Log("Tower Moved");
                _willBlockAgent = false;
                return true;
            }
            return false;
        }
        
        
    } 
}