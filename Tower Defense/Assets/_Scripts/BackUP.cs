
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


namespace _Scripts
{
    public class BackUP : MonoBehaviour
    {
        [SerializeField] private GameObject transparentTowerPrefab, towerPrefab, towerDummyPrefab;
        // [SerializeField] private Button archerButton;

        private const float GridSize = 2.0f;
        private bool _archerButtonIsPressed;
        private GameObject _currentTransparentTowerInstance, _currentTransparentTowerInstaceDummy;
        private GameObject _instantiatedTransparentTower;
        private GameObject _instantiatedTransparentTowerDummy;
        private bool _transparentTowerIsActive;
        public bool currentColor;

        private Renderer _rendererTransparentTower;
        private NavMeshLink _transparentTowerNavMeshLink;
        private NavMeshObstacle _dummyNavMeshObstacle;

        public LayerMask groundLayer;
        public LayerMask towerLayer;
        public LayerMask enemyLayer;

        private Vector3 _dummyLastPosition;
        

        //[SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private DummyEnemy dummyEnemy;
    
        

        // Update is called once per frame
        private void Update()
        {
            if (HasMoved())
            {
                dummyEnemy.ValidatePath();
            }
            
            RaycastHit hit;
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, groundLayer + towerLayer)) //Sends a ray cast from the camera and checks if it hits any objects in ground and tower layer
                {
                    if (_archerButtonIsPressed) // Bool what tower is currently selected.
                    {
                       
                        
                        //GameObject tower1 = hit.transform.gameObject;
                        if (Input.GetKeyDown(KeyCode.Mouse0) && currentColor) //User input Left-Mouse button. Bool currentColor to check is the tower can be placed.
                        {
                   
                            InstantiateTower(towerPrefab, hit); // instantiate the tower prefab at the raycast hit position
                            Destroy(_instantiatedTransparentTower); // Remove the transparent tower once a real one is placed
                            Destroy(_instantiatedTransparentTowerDummy); // Remove the transparent tower dummy once a real one is placed
                            _transparentTowerIsActive = false;
                        }
                        else if (!_transparentTowerIsActive)
                        {
                            _transparentTowerIsActive = true;
                            _instantiatedTransparentTower = InstantiateTransparentTower(transparentTowerPrefab ,hit); // Only create it once when the player pressed a button
                            _instantiatedTransparentTowerDummy = InstantiateDummy(towerDummyPrefab, hit);
                        }
                        if(_instantiatedTransparentTower != null && _transparentTowerIsActive)
                        {
                            
                            var gridPos = SnapToGrid(hit.point, GridSize); // Makes the transparent tower follow the players mouse
                            _instantiatedTransparentTower.transform.position = gridPos; // Makes the transparent tower follow the players mouse
                            _instantiatedTransparentTowerDummy.transform.position = gridPos; // Makes the transparent dummy tower follow the players mouse
                            
                    
                    
                            GameObject tower = hit.transform.gameObject;
                            if (tower.CompareTag("Tower") || tower.CompareTag("Enemy") || !dummyEnemy.canReachDestinationDummy)
                            {
                                currentColor = false;
                                ChangeColor(_instantiatedTransparentTower, Color.red);
                        
                                if (dummyEnemy.canReachDestinationDummy)
                                {
                                    
                                    _transparentTowerNavMeshLink.enabled = false; 
                                    
                                }
                                else if (!dummyEnemy.canReachDestinationDummy)
                                {
                                    _transparentTowerNavMeshLink.enabled = true; 
                                }
                                if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, enemyLayer )) 
                                {
                                    _dummyNavMeshObstacle.enabled = false;
                                }
                                else
                                {
                                    _dummyNavMeshObstacle.enabled = true;
                                }
                            }
                            if (tower.CompareTag("Ground") && dummyEnemy.canReachDestinationDummy)
                            {
                                
                                StartCoroutine(PauseForPointOneSecond());
                            }
                        } 
                    }
                }
        }

    
        private void InstantiateTower(GameObject tower, RaycastHit hit)
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            Instantiate(tower, gridPos, Quaternion.identity);
        }

        private GameObject InstantiateTransparentTower(GameObject transparentTower, RaycastHit hit) //instantiate transparent tower
        {
            _transparentTowerIsActive = true;
       
            var gridPos = SnapToGrid(hit.point, GridSize);
            // Make sure to return the instantiated object
            GameObject towerInstance = Instantiate(transparentTower, gridPos, Quaternion.identity);
            _rendererTransparentTower = towerInstance.GetComponent<Renderer>();
            _transparentTowerNavMeshLink = towerInstance.GetComponent<NavMeshLink>();
            return towerInstance;
        
        }
        private GameObject InstantiateDummy(GameObject dummyTower, RaycastHit hit) //Instantiate dummy tower
        {
            _transparentTowerIsActive = true;
       
            var gridPos = SnapToGrid(hit.point, GridSize);
            // Make sure to return the instantiated object
            GameObject dummyTowerInstance = Instantiate(dummyTower, gridPos, Quaternion.identity);
            _dummyNavMeshObstacle = dummyTowerInstance.GetComponent<NavMeshObstacle>();
        
            return dummyTowerInstance;
        
        }

        private Vector3 SnapToGrid(Vector3 rawWorldPos, float gridSize) //Snap to grid function for towers
        {
            int x = Mathf.RoundToInt(rawWorldPos.x / gridSize);
            int y = Mathf.RoundToInt(rawWorldPos.y / gridSize);
            int z = Mathf.RoundToInt(rawWorldPos.z / gridSize);
        
            return new Vector3(x * gridSize, y * gridSize, z * gridSize);
        }


        public void PlayerButtonInputArcher() //Public bool so that it can be accessed through unity's button manager.
        {
            _archerButtonIsPressed = true;
        }
    
    
    
        void ChangeColor(GameObject obj, Color newColor) //Changes the color of whatever Gameobject you want.
        {
            if (obj != null)
            {
            
                if (_rendererTransparentTower != null && _rendererTransparentTower.sharedMaterial != null)
                {
                    _rendererTransparentTower.sharedMaterial.color = newColor;
                }
            }
        }
        private IEnumerator PauseForPointOneSecond() //Wait for 0.1 seconds before the bool currentColor is set to true.
        {
            
            yield return new WaitForSeconds(0.1f);
            ChangeColor(_instantiatedTransparentTower, Color.green);
            currentColor = true;
        }


        private bool HasMoved() //Checks if the dummy tower has moved position
        {
            if (_instantiatedTransparentTowerDummy == null)
            {
                Debug.Log("Tower Dummy is null");
                return false;
            }
            if (_dummyLastPosition != _instantiatedTransparentTowerDummy.transform.position)
            {
                _dummyLastPosition = _instantiatedTransparentTowerDummy.transform.position;
                Debug.Log("Dummy Tower Moved");
                return true;
            }
            return false;
        }


    } //Inside parameter  
    
}//namespace
