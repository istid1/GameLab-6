using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public Camera mainCam;
    private const string GroundTag = "Ground";
    private const string TowerTag = "Tower";
    public GameObject towerPrefab;
    private const int MouseButtonLeft = 0;
    private const float GridSize = 2.0f;
    private GameObject _currentTransparentTower;
    public GameObject transparentTowerPrefab;
    private Grid _grid;
    private bool _mouseIsHeldDown;

    private void Update()
    {
        if (Input.GetMouseButtonDown(MouseButtonLeft) && !_mouseIsHeldDown && TryGetHitFromMousePosition(out RaycastHit hit))
        {
            _mouseIsHeldDown = true;
            CheckAndInstantiateTower(hit, true);
        }
        else if (Input.GetMouseButton(MouseButtonLeft) && _mouseIsHeldDown && _currentTransparentTower != null && TryGetHitFromMousePosition(out hit))
        {
            var gridPos = SnapToGrid(hit.point, GridSize);
            _currentTransparentTower.transform.position = gridPos;
        }
        else if (Input.GetMouseButtonUp(MouseButtonLeft))
        {
            _mouseIsHeldDown = false;
            _currentTransparentTower = null;
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
            _currentTransparentTower = Instantiate(prefab, gridPos, Quaternion.identity);
        }
    }
    
    private bool TryGetHitFromMousePosition(out RaycastHit hit)
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit);
    }
    
    private bool IsGround(Collider groundCollider) => groundCollider.CompareTag(GroundTag);
    
    private bool IsTower(Collider towerCollider) => towerCollider.CompareTag(TowerTag);
    
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
            
        
    

