using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;


namespace _Scripts
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] private GameObject transparentTowerPrefab, towerPrefab;
        private const float GridSize = 2.0f;
        private bool _archerButtonIsPressed;
        private GameObject _currentTransparentTowerInstance;
        private GameObject _instantiatedTransparentTower;
        private GameObject[] allEnemies;
        private GameObject blockingTower = null;
        
        
        private bool _transparentTowerIsActive;
        public bool currentColor;
        private Renderer _rendererTransparentTower;
        [SerializeField] private NavMeshSurface[] navMeshSurfaces;
        
        public LayerMask groundLayer;
        public LayerMask towerLayer;
        public LayerMask enemyLayer;

        private Vector3 _transparentTowerLastPos;
        [SerializeField] private List<GameObject> placedTower = new List<GameObject>();
        private EnemyMovement _enemyMovement;


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
            
            
            allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DestroyLastTowerInList();
            }
          

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

        private bool RaycastHitsLayer(Ray ray, LayerMask layer, out RaycastHit hit)
        {
            return Physics.Raycast(ray, out hit, Mathf.Infinity, layer);
        }

        private void HandleArcherTowerSelection(RaycastHit hit)
        {
            if (_archerButtonIsPressed)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && currentColor)
                {
                    PlaceTower(hit);
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
            placedTower.Add(newTower);
    
            if (_instantiatedTransparentTower != null)
            {
                Destroy(_instantiatedTransparentTower);
                _instantiatedTransparentTower = null; // Avoid memory leak
            }
    
            _transparentTowerIsActive = false;
            
            blockingTower = newTower;     // Store the reference to the newly placed Tower which might block the path
        }

        private void SpawnTransparentTower(RaycastHit hit)
        {
            _instantiatedTransparentTower = InstantiateTransparentTower(transparentTowerPrefab, hit);
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
            _instantiatedTransparentTower.transform.position = gridPos;
           
        }

        private void CheckAndHandleObstacles(RaycastHit hit)
        {
            //Debug.DrawRay(hit.point, -mouseRay.direction * 10, Color.yellow);
           
            
            GameObject tower = hit.transform.gameObject;
            if (tower.CompareTag("Tower"))
            {
                HandleObstruction();
            }
            if (tower.CompareTag("Ground"))
            {
               
                ChangeColor(_instantiatedTransparentTower, Color.green);
                currentColor = true;
                
            }
        }

     

        private void HandleObstruction()
        {
            
            ChangeColor(_instantiatedTransparentTower, Color.red);
            currentColor = false;
        }

        private GameObject InstantiateTower(GameObject tower, RaycastHit hit)
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            GameObject newTower = Instantiate(tower, gridPos, Quaternion.identity);
            return newTower;
        }
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

        public void PlayerButtonInputArcher()
        {
            _archerButtonIsPressed = true;
        }

        void ChangeColor(GameObject obj, Color newColor)
        {
            if (obj != null)
            {
                if (_rendererTransparentTower != null && _rendererTransparentTower.sharedMaterial != null)
                {
                    _rendererTransparentTower.sharedMaterial.color = newColor;
                }
            }
        }

        private void DestroyLastTowerInList()
        {
            if (placedTower.Count > 0)
            {
                // get the last tower
                var lastTower = placedTower[placedTower.Count - 1];
        
                // remove it from the list
                placedTower.Remove(lastTower);
        
                // destroy the tower object
                Destroy(lastTower);
            }
            else
            {
                // handle the case where there are no towers
                
            }
            
        }

        private void EnemiesCantFinishPath()
        {
            Debug.Log("Invoked EnemiesCantFinishPath()");

            if (allEnemies == null) 
            {
                Debug.Log("allEnemies is null");
                return;
            }
    
            foreach(var enemy in allEnemies)
            {
                Debug.Log("Checking enemy");
                var enemyMovement = enemy.GetComponent<EnemyMovement>();

                if(enemy == null)
                {
                    Debug.Log("Enemy is null");
                    continue;
                }
                
                if( !enemyMovement.canReachDestination)
                {
                    Debug.Log("Enemy can't reach destination");   
                    DestroyBlockTower();
                    return;
                    
                }
            }
            
        }
        
        private void DestroyBlockTower()
        {
            if (blockingTower != null)
            {
                placedTower.Remove(blockingTower);    // remove from the list
                Destroy(blockingTower);              // destroy the tower object
                blockingTower = null;                // Nullify the reference to avoid deleting the same tower multiple times
            }
            else
            {
                // Handle the case where there's no blocking tower
            }
        }
        
        
    } 
}