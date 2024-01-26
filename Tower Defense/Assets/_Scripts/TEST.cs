using UnityEngine;
using System.Collections.Generic;

public class TEST : MonoBehaviour
{
    // The following variables have not been changed
    public Camera mainCam;
    private const string GroundTag = "Ground";
    private const string TowerTag = "Tower";
    public GameObject towerPrefab;
    private const int MouseButtonLeft = 0;
    private const float GridSize = 2.0f;
    public GameObject transparentTowerPrefab;
    private bool _mouseIsHeldDown;
    private bool archerButtonIsPressed = false;
    private bool currentColor;

    private GameObject _currentTransparentTower;

    private void Update()
    {
        if (archerButtonIsPressed)
        {
            ManageMouseClicks();
        }
    }

    private void ManageMouseClicks()
    {
        if (!_mouseIsHeldDown && TryGetHitFromMousePosition(out RaycastHit hit))
        {
            _mouseIsHeldDown = true;

            if (CheckConditions(hit, false))
            {
                InstantiateTower(hit, true);
            }
        }
        else if (_mouseIsHeldDown && _currentTransparentTower != null && TryGetHitFromMousePosition(out hit))
        {
            // The body of this else if clause is unchanged
            // .....
        }
        else if (Input.GetMouseButtonDown(MouseButtonLeft))
        {
            HandleLeftMouseButtonDown();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DestroyTower();
        }
    }

    private void HandleLeftMouseButtonDown()
    {
        if (TryGetHitFromMousePosition(out RaycastHit hit))
        {
            _mouseIsHeldDown = false;
            DestroyAllTransparentTowers();
            
            if (currentColor)
            {
                Debug.Log("Ground hit detected. Instantiating tower...");
                InstantiateTower(hit, false);
            }
        }
    }

    private void InstantiateTower(RaycastHit hit, bool transparent)
    {
        var gridPos = SnapToGrid(hit.point, GridSize);
        var towerToInstantiate = transparent ? transparentTowerPrefab : towerPrefab;
        _currentTransparentTower = Instantiate(towerToInstantiate, gridPos, Quaternion.identity);
    }
    
    private bool TryGetHitFromMousePosition(out RaycastHit hit)
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit);
    }
    
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
    
    private void DestroyAllTransparentTowers()
    {
        foreach (var transparentTower in GameObject.FindGameObjectsWithTag("TransparentTower"))
        {
            Destroy(transparentTower);
        }
    }
    
    private bool IsTower(Collider towerCollider) => towerCollider.CompareTag(TowerTag);
    private bool IsGround(Collider groundCollider) => groundCollider.CompareTag(GroundTag);
    
    
    private bool CheckConditions(RaycastHit hit, bool onlyGroundCheck)
    {
        if (towerPrefab == null) 
        {
            Debug.LogError("Tower Prefab is not assigned.");
            return false;
        }
        if (!IsGround(hit.collider)) return false;
        if (!onlyGroundCheck && HitDetectsTower(hit.point))
        {
            return false;
        }
        return true;
    }
    
    
    private bool HitDetectsTower(Vector3 point)
    {
        if (Physics.Raycast(point, Vector3.up, out RaycastHit hitTower))
        {
            return hitTower.transform.CompareTag("Tower");
        }
        return false;
    }
    
    
    public void ButtonSelect()
    {
        archerButtonIsPressed = true;
       
    }
    
}
    
    