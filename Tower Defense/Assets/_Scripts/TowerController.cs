using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.VFX;


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

        [SerializeField] private DummyEnemy _dummyEnemy;

        private bool _archerButtonIsPressed, _fireButtonIsPressed, _iceButtonIsPressed, _lightningButtonIsPressed, _bombButtonIsPressed;
        private bool _transparentTowerIsActive;
        private bool _willBlockAgent;
        private bool _runOnce;
        private bool _currentColor;
        [SerializeField] private TMP_Text _currTowerText;
        [SerializeField] private TMP_Text _fpsCounter;
        private TowerVariables _towerVariables;
        [SerializeField] private UpgradeCanvasAnimation _upgradeCanvasAnimation;
        
        [SerializeField ] private GameObject _dummyEnemyEnabler;
        private GameObject _currentTransparentTowerInstance;
        private GameObject _instantiatedTransparentTower;
        private GameObject _blockingTower;
        [SerializeField] public GameObject _upgradeCanvas;

        private Renderer _rendererTransparentTower;
        private EnemyMovement _enemyMovement;
        private List<EnemyMovement> _enemyMovements;
        private GameObject _currentSelectedTower;
        public LayerMask groundLayer;
        public LayerMask towerLayer;
        public LayerMask enemyLayer;

        [SerializeField] private MoneySystem _moneySystem;

        // Different costs for towers
        [SerializeField] private int _costBallistaTower = 50;
        [SerializeField] private int _costFireTower = 100;
        [SerializeField] private int _costIceTower = 150;
        [SerializeField] private int _costLightningTower = 200;
        [SerializeField] private int _costBombTower = 250;

        private float _fpsTimer = 0;
        //private int _upgradeCost = 50;
        private bool canAfordUpgrade;
        
        
        private void Awake()
        {
            
            BuildNavMeshSurfaces();
           _mainCamera = Camera.main;
           _dummyEnemyEnabler.SetActive(true);
        }

        private void Start()
        {
            Application.targetFrameRate = 144; 
        }

        // Update is called once per frame
        private void Update()
        {
            
            FPSCounter();
            CheckColor();
            HasMoved();
            UserInputRotateTower();
            EnemiesCantFinishPath();
            DummyCantFinishPath();
            
            if (_instantiatedTransparentTower == null)
            {
                RayCastSelectTower();
            }
            
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

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                ResetButtonStates();
                Destroy(_instantiatedTransparentTower);
                
            }

            if (_moneySystem.currentMoney >= 50)
            {
                canAfordUpgrade = true;
            }

            if (_moneySystem.currentMoney < 50)
            {
                canAfordUpgrade = false;
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
            int cost = GetCurrentTowerCost();
            
            
            if (_moneySystem.currentMoney < cost)
            {
                return;
            }
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

            _moneySystem.currentMoney -= cost;

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
            if (tower.CompareTag("Tower") || tower.CompareTag("UpgradeTag"))
            {
                HandleObstruction();
            }
            if (tower.CompareTag("Ground") && !_willBlockAgent && _dummyEnemy.canReachDestinationDummy)
            {
                _currentColor = true;
            }
            if (!_dummyEnemy.canReachDestinationDummy)
            {
                _currentColor = false;
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
        
        private int GetCurrentTowerCost()
        {
            // Based on which button is pressed return the cost of the corresponding tower
            if (_archerButtonIsPressed) return _costBallistaTower;
            if (_fireButtonIsPressed) return _costFireTower;
            if (_iceButtonIsPressed) return _costIceTower;
            if (_lightningButtonIsPressed) return _costLightningTower;
            if (_bombButtonIsPressed) return _costBombTower;
            
            // Return some default value or throw an exception if no button is pressed  
            return 0;
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
            
            if (_moneySystem.currentMoney < _costBallistaTower)
            {
                Debug.Log("Can't afford");
                return;
            }
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
            if (_moneySystem.currentMoney < _costFireTower)
            {
                Debug.Log("Can't afford");
                return;
            }
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
            if (_moneySystem.currentMoney < _costIceTower)
            {
                Debug.Log("Can't afford");
                return;
            }
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
            if (_moneySystem.currentMoney < _costLightningTower)
            {
                Debug.Log("Can't afford");
                return;
            }
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
            if (_moneySystem.currentMoney < _costBombTower)
            {
                Debug.Log("Can't afford");
                return;
            }
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
                return;
            }

            if (_enemyMovements.All(enemyMovements => enemyMovements.canReachDestination)) return;
            DestroyBlockTower();
            _willBlockAgent = true;
            
        }
        
        private void DummyCantFinishPath()
        {
            if (_dummyEnemy.canReachDestinationDummy) return;
            DestroyBlockTower();
            _willBlockAgent = true;
        }
        
        
        
        private void DestroyBlockTower()
        {
            if (_blockingTower == null) return;                 // null check
            placedTower.Remove(_blockingTower);                 // remove the Tower from the list
            _moneySystem.currentMoney += GetCurrentTowerCost(); // Refund the cost of the tower if the tower will block the enemies path 
            Destroy(_blockingTower);                            // destroy the Tower object
            _blockingTower = null;                              // Nullify the reference to avoid deleting the same tower multiple times
            ResetButtonStates();                                // Reset current selected tower button
            Destroy(_instantiatedTransparentTower);             // destroy the transparent tower
            
        }
        
        
        private void HasMoved() //Checks if the mouse has moved position to another grid space
        {
            if (_instantiatedTransparentTower == null)
            {
                return;
            }

            if (_mouseLastPosition == _instantiatedTransparentTower.transform.position) return;
            _mouseLastPosition = _instantiatedTransparentTower.transform.position;
            
            _willBlockAgent = false;
        }
        
        private void RayCastSelectTower()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                _upgradeCanvasAnimation.MoveCanvasDisabled();
                _currentSelectedTower = null;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit))
                {
                    
                    if(hit.transform.gameObject.tag == "UpgradeTag")
                    {
                        _currentSelectedTower = hit.transform.gameObject;
                        _currTowerText.text = _currentSelectedTower.ToString();
                        _upgradeCanvas.SetActive(true);
                        _upgradeCanvasAnimation.MoveCanvasActive();
                    }

                    if (hit.transform.gameObject.tag == "Ground" || hit.transform.gameObject.tag == "Enemy")
                    {
                        _currentSelectedTower = null;
                        _upgradeCanvasAnimation.MoveCanvasDisabled();
                    }
                }
            }
        }

        public void DeSelect()
        {
            if (_instantiatedTransparentTower == null)
            {
                _upgradeCanvasAnimation.MoveCanvasDisabled();
                _currentSelectedTower = null;
            }
        }

        private void UpgradeSelectedTowerDamage()
        {
            if (_currentSelectedTower != null)
            {
                var towerVariables = _currentSelectedTower.GetComponent<TowerVariables>();
                towerVariables.UpgradeDamage();
            }
        }
        private void UpgradeSelectedTowerRange()
        {
            if (_currentSelectedTower != null)
            {
                var towerVariables = _currentSelectedTower.GetComponent<TowerVariables>();
                towerVariables.UpgradeRange();
            }
        }
        private void UpgradeSelectedTowerFireRate()
        {
            if (_currentSelectedTower != null)
            {
                var towerVariables = _currentSelectedTower.GetComponent<TowerVariables>();
                towerVariables.UpgradeFireRate();
            }
        }

        public void DamageUpgradeButton()
        {
            if (canAfordUpgrade)
            {
                if (_currentSelectedTower == null)
                {
                    return;
                }
                var towerVariables = _currentSelectedTower.GetComponent<TowerVariables>();
            
                if (towerVariables == null)
                {
                    return;
                }
                UpgradeSelectedTowerDamage();
            
                if (!towerVariables.damageIsUpgraded)
                {
                    PlayUpgradeVFX();
                }
            }
           
        }

        public void RangeUpgradeButton()
        {
            if (canAfordUpgrade)
            {
                if (_currentSelectedTower == null)
                {
                    return;
                }
                var towerVariables = _currentSelectedTower.GetComponent<TowerVariables>();
            
                if (towerVariables == null)
                {
                    return;
                }
                UpgradeSelectedTowerRange();
            
                if (!towerVariables.rangeIsUpgraded)
                {
                    PlayUpgradeVFX();
                }
            }
           
        }

        public void FireRateUpgradeButton()
        {
            if (canAfordUpgrade)
            {
                if (_currentSelectedTower == null)
                {
                    return;
                }
                var towerVariables = _currentSelectedTower.GetComponent<TowerVariables>();
            
                if (towerVariables == null)
                {
                    return;
                }
                UpgradeSelectedTowerFireRate();
            
                if (!towerVariables.fireRateIsUpgraded)
                {
                    PlayUpgradeVFX();
                }
            }
        }

        public void SellButton()
        {
            
            if (_currentSelectedTower == null)
            {
                return;
            }
            
            var currentTowerType = _currentSelectedTower.GetComponent<TowerTypeing>();


            int currentUpgradeLevelDamage =
                _currentSelectedTower.GetComponent<TowerVariables>()._currentDamageUpgradeLevel;
            int currentUpgradeLevelFireRate =
                _currentSelectedTower.GetComponent<TowerVariables>()._currentFireRateUpgradeLevel;
            int currentUpgradeLevelRange =
                _currentSelectedTower.GetComponent<TowerVariables>()._currentRangeUpgradeLevel;
         
            int baseRefund = 0;
            if (currentTowerType._towerTypeString == "BallistaTower")
            {
                baseRefund = 50 / 2;
            }
            else if (currentTowerType._towerTypeString == "FireTower")
            {
                baseRefund = 100 / 2;
            }
            else if (currentTowerType._towerTypeString == "IceTower")
            {
                baseRefund = 150 / 2;
            }
            else if (currentTowerType._towerTypeString == "LightningTower")
            {
                baseRefund = 200 / 2;
            }
            else if (currentTowerType._towerTypeString == "BombTower")
            {
                baseRefund = 250 / 2;
            }

            int totalRefund = baseRefund + ((currentUpgradeLevelDamage + currentUpgradeLevelRange + currentUpgradeLevelFireRate) * 25);
            
            _moneySystem.currentMoney += totalRefund;
            
            placedTower.Remove(_currentSelectedTower);
            Destroy(_currentSelectedTower);
            
        }

        private void PlayUpgradeVFX()
        {
            
            var vfxs = _currentSelectedTower.GetComponentsInChildren<VisualEffect>();
            foreach (var vfx in vfxs)
            {
                if (vfx.gameObject.tag != "Lightning" && vfx.gameObject.tag != "BombFireVFX")
                {
                    vfx.Play();
                }
            }
        }


        private void FPSCounter()
        {
            _fpsTimer += Time.deltaTime;
            if (!(_fpsTimer >= 1f)) return;
            _fpsCounter.text = "FPS : " + ((int)(1f / Time.deltaTime));
            _fpsTimer = 0;
        }

        
        
        
        
        
        
        
    } //outside Class
} // outside Namespace