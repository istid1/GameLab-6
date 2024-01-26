using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
    private bool archerButtonIsPressed = false;
    
    private bool currentColor;
    
    private void Update()
    {

        if (archerButtonIsPressed)
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
            var gridPos = SnapToGrid(hit.point, GridSize);
            _currentTransparentTower.transform.position = gridPos;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    
            if (Physics.Raycast(ray, out hit))
            {
                GameObject tower = hit.transform.gameObject;
                if (tower.CompareTag("Tower"))
                {
                    ChangeColor(_currentTransparentTower, Color.red);
                    currentColor = false;
                }
                else if (tower.CompareTag("Ground"))
                {
                    ChangeColor(_currentTransparentTower, Color.green);
                    currentColor = true;
                }
            }
        }
        else if (Input.GetMouseButtonDown(MouseButtonLeft))
        {
            if (TryGetHitFromMousePosition(out hit))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                _mouseIsHeldDown = false;
                DestroyAllTransparentTowers();

                if (currentColor)
                {
                    Debug.Log("Ground hit detected. Instantiating tower...");
                    InstantiateTower(hit, false); 
                }
            }
        }
            else if (Input.GetMouseButtonDown(1))
            { 
                DestroyTower();
            }
        }
        
        
        
    }


    private void DestroyAllTransparentTowers()
    {
        foreach (var transparentTower in GameObject.FindGameObjectsWithTag("TransparentTower"))
        {
            Destroy(transparentTower);
        }
    }
    
    
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
    
  


    private void InstantiateTower(RaycastHit hit, bool transparent)
    {
        var gridPos = SnapToGrid(hit.point, GridSize);
        var prefab = transparent ? transparentTowerPrefab : towerPrefab;
        _currentTransparentTower = Instantiate(prefab, gridPos, Quaternion.identity);
    }
    
    
    void ChangeColor(GameObject obj, Color newColor)
    {
        obj.GetComponent<Renderer>().material.color = newColor;
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


    public void ButtonSelect()
    {
        archerButtonIsPressed = true;
       
    }
    
    
    
    
    
    
    
}
            
        
    

