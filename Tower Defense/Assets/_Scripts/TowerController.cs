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
        
        private bool _transparentTowerIsActive;
        public bool currentColor;
        private Renderer _rendererTransparentTower;
        [SerializeField] private NavMeshSurface[] navMeshSurfaces;
        
        public LayerMask groundLayer;
        public LayerMask towerLayer;
        public LayerMask enemyLayer;

        private Vector3 _transparentTowerLastPos;


        private void Awake()
        {
            BuildNavMeshSurfaces();
            
            
            
        }
        
        
        
        

        private void BuildNavMeshSurfaces()
        {
            for (int i = 0; i < navMeshSurfaces.Length; i++)
            {
             navMeshSurfaces[i].BuildNavMesh();
             Debug.Log("NavMesh Rebuilt");
            }
        }
        

        // Update is called once per frame
        private void Update()
        {
            Debug.Log(currentColor);
            
            FindEnemies();
            
          

            if (Camera.main != null)
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (RaycastHitsLayer(mouseRay, groundLayer + towerLayer, out var hit))
                {
                    HandleArcherTowerSelection(hit);
                }
            }
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
            
            InstantiateTower(towerPrefab, hit);
            Destroy(_instantiatedTransparentTower);
            
            _transparentTowerIsActive = false;
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
            Debug.Log("Raycast hit: " + hit.collider.gameObject.tag);
            
            GameObject tower = hit.transform.gameObject;
            if (tower.CompareTag("Tower"))
            {
                HandleObstruction();
            }
            if (tower.CompareTag("Ground"))
            {
                Debug.Log("Change Color Function Called"); // Debug Statement
                ChangeColor(_instantiatedTransparentTower, Color.green);
                currentColor = true;
                
            }
        }

     

        private void HandleObstruction()
        {
            
            ChangeColor(_instantiatedTransparentTower, Color.red);
            currentColor = false;
        }

        private void InstantiateTower(GameObject tower, RaycastHit hit)
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            Instantiate(tower, gridPos, Quaternion.identity);
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


        private void FindEnemies()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach(GameObject enemy in enemies)
            {
                if(enemy != null)
                {
                    EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        
                    if(enemyMovement != null)
                    {
                        // You can now access the `canReachDestination` property for each enemy
                    }
                    else
                    {
                        //Debug.LogWarning("EnemyMovement component missing on enemy object");
                    }
                }
                else 
                {
                    //Debug.LogWarning("No enemy objects found with the 'Enemy' tag");
                }
            }
        }
    } 
}