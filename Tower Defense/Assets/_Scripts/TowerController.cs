
using _Scripts;
using UnityEngine;

using Button = UnityEngine.UI.Button;

public class TowerController : MonoBehaviour
{
    [SerializeField] private GameObject transparentTowerPrefab, towerPrefab, towerDummyPrefab;
    [SerializeField] private Button archerButton;

    private const float GridSize = 2.0f;
    private bool _archerButtonIsPressed;
    private GameObject currentTransparentTowerInstance, currentTransparentTowerInstaceDummy;
    private GameObject instantiatedTransparentTower;
    private GameObject instantiatedTransparentTowerDummy;
    private bool transparentTowerIsActive;
    public bool currentColor;

    private Renderer _rendererTransparentTower;

    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private DummyEnemy _dummyEnemy;
    
    public void ArcherButtonIsPressed()
    {
        _archerButtonIsPressed = true;
    }

    // Update is called once per frame
    private void Update()
    {
        
        
        RaycastHit hit;
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out hit)) 
        {
            if (_archerButtonIsPressed)
            {
                GameObject tower1 = hit.transform.gameObject;
                if (Input.GetKeyDown(KeyCode.Mouse0) && currentColor)
                {
                   
                    InstantiateTower(towerPrefab, hit);
                   // InstantiateTower(towerDummyPrefab, hit);
                    Destroy(instantiatedTransparentTower); // Remove the transparent tower once a real one is placed
                    Destroy(instantiatedTransparentTowerDummy); // Remove the transparent tower dummy once a real one is placed
                    transparentTowerIsActive = false;
                }
                else if (!transparentTowerIsActive)
                {
                    transparentTowerIsActive = true;
                    instantiatedTransparentTower = InstantiateTransparentTower(transparentTowerPrefab ,hit); // Only create it once when the player pressed a button
                    instantiatedTransparentTowerDummy = InstantiateDummy(towerDummyPrefab, hit);
                }

                if(instantiatedTransparentTower != null && transparentTowerIsActive)
                {
                    // Move your transparent tower towards your mouse
                    var gridPos = SnapToGrid(hit.point, GridSize);
                    instantiatedTransparentTower.transform.position = gridPos;
                    instantiatedTransparentTowerDummy.transform.position = gridPos;
                    
                    
                    GameObject tower = hit.transform.gameObject;
                    if (tower.CompareTag("Tower") || tower.CompareTag("Enemy") || _dummyEnemy.canReachDestinationDummy == false )
                    {
                        currentColor = false;
                        ChangeColor(instantiatedTransparentTower, Color.red);
                        
                    }
                    else if (tower.CompareTag("Ground") && _dummyEnemy.canReachDestinationDummy)
                    {
                        currentColor = true;
                        ChangeColor(instantiatedTransparentTower, Color.green);
                        
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

    private GameObject InstantiateTransparentTower(GameObject transparentTower, RaycastHit hit)
    {
        transparentTowerIsActive = true;
       
        var gridPos = SnapToGrid(hit.point, GridSize);
        // Make sure to return the instantiated object
        GameObject towerInstance = Instantiate(transparentTower, gridPos, Quaternion.identity);
        _rendererTransparentTower = towerInstance.GetComponent<Renderer>();
        return towerInstance;
        
    }
    private GameObject InstantiateDummy(GameObject DummyTower, RaycastHit hit)
    {
        transparentTowerIsActive = true;
       
        var gridPos = SnapToGrid(hit.point, GridSize);
        // Make sure to return the instantiated object
        GameObject dummyTowerInstance = Instantiate(DummyTower, gridPos, Quaternion.identity);
        return dummyTowerInstance;
        
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
}
