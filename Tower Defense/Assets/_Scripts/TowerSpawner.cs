using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public Camera mainCam;
    private const string GroundTag = "Ground";
    private const string TowerTag = "Tower";
    public GameObject towerPrefab;
    private const int MouseButtonLeft = 0;
    private const float GridSize = 2.0f;
    private GameObject currentTransparentTower = null;
    public GameObject transparentTowerPrefab;
    private Grid grid;
    private bool mouseIsHeldDown = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(MouseButtonLeft) && !mouseIsHeldDown && TryGetHitFromMousePosition(out RaycastHit hit))
        {
            mouseIsHeldDown = true;
            CheckAndInstantiateTower(hit, true);
        }
        else if (Input.GetMouseButton(MouseButtonLeft) && mouseIsHeldDown && currentTransparentTower != null && TryGetHitFromMousePosition(out hit))
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            currentTransparentTower.transform.position = gridPos;
        }
        else if (Input.GetMouseButtonUp(MouseButtonLeft))
        {
            mouseIsHeldDown = false;
            currentTransparentTower = null;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DestroyTower();
        }
    }

    private void CheckAndInstantiateTower(RaycastHit hit, bool transparent)
    {
        if (towerPrefab == null) 
        {
            Debug.LogError("Tower Prefab is not assigned.");
            return;
        }
        if (IsGround(hit.collider))
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            var prefab = transparent ? transparentTowerPrefab : towerPrefab;
            currentTransparentTower = Instantiate(prefab, gridPos, Quaternion.identity);
        }
    }
    
    private bool TryGetHitFromMousePosition(out RaycastHit hit)
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit);
    }
    
    private bool IsGround(Collider groundCollider) => groundCollider.tag == GroundTag;
    
    private bool IsTower(Collider towerCollider) => towerCollider.tag == TowerTag;
    
    private void DestroyTower()
    {
        if(TryGetHitFromMousePosition(out RaycastHit hit) && IsTower(hit.collider))
            Destroy(hit.collider.gameObject);        
    }
    
    private Vector3 SnapToGrid(Vector3 rawWorldPos, float gridSize)
    {
        int x = Mathf.RoundToInt(rawWorldPos.x / gridSize);
        int y = Mathf.RoundToInt(rawWorldPos.y / gridSize);
        int z = Mathf.RoundToInt(rawWorldPos.z / gridSize);
        return new Vector3(x * gridSize, y * gridSize, z * gridSize);
    }
}
            
        
    

