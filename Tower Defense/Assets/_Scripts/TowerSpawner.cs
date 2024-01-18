using UnityEngine;
using UnityEngine.Rendering;

public class TowerSpawner : MonoBehaviour
{
    public Camera mainCam;
    private const string GroundTag = "Ground";
    private const string TowerTag = "Tower";
    public GameObject towerPrefab;

    public GameObject transparentTowerPrefab;
    //[SerializeField] private GameObject cellIndicator;
    private Grid grid;
    
    private bool mouseIsHeldDown = false;
    
    void Update()
    {
       
    
        
        
        
        
        
        
        
        
        
        if (Input.GetMouseButtonUp(0))
        {
            mouseIsHeldDown = false;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CheckAndInstantiateTower(hit);
                
            }
        }
        
        if (Input.GetMouseButton(0) && !mouseIsHeldDown)
        {
            mouseIsHeldDown = true;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CheckAndInstantiateTransparentTower(hit);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            DestroyTower();
        }
    }

    private void CheckAndInstantiateTransparentTower(RaycastHit hit)
    {
        if (towerPrefab == null) 
        {
            Debug.LogError("Tower Prefab is not assigned.");
            return;
        }
    
        if (IsGround(hit.collider))
        {
            float gridSize = 2.0f; // Replace with your actual grid size
            Vector3 gridPos = SnapToGrid(hit.point, gridSize);
            Instantiate(transparentTowerPrefab, gridPos, Quaternion.identity);
            
        }
    }
    
    private void CheckAndInstantiateTower(RaycastHit hit)
    {
        if (towerPrefab == null) 
        {
            Debug.LogError("Tower Prefab is not assigned.");
            return;
        }
    
        if (IsGround(hit.collider))
        {
            float gridSize = 2.0f; // Replace with your actual grid size
            Vector3 gridPos = SnapToGrid(hit.point, gridSize);
            Instantiate(towerPrefab, gridPos, Quaternion.identity);
        }
    }
    
    

    private bool IsGround(Collider groundCollider)
    {
        return groundCollider.tag == GroundTag;
    }
    private bool IsTower(Collider towerCollider)
    {
        return towerCollider.tag == TowerTag;
    }

    private void DestroyTower()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (IsTower(hit.collider))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    
    private Vector3 SnapToGrid(Vector3 rawWorldPos, float gridSize)
    {
        int x = Mathf.RoundToInt(rawWorldPos.x / gridSize);
        int y = Mathf.RoundToInt(rawWorldPos.y / gridSize);
        int z = Mathf.RoundToInt(rawWorldPos.z / gridSize);

        return new Vector3(x * gridSize, y * gridSize, z * gridSize);
    }
    
    
    
    
    
}
            
        
    

