using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class TowerController : MonoBehaviour
{
    [SerializeField] private GameObject transparentTower, towerPrefab;
    [SerializeField] private Button archerButton;

    private const float GridSize = 2.0f;
    private bool archerButtonIsPressed;
    private GameObject currentTransparentTowerInstance;
    private GameObject instantiatedTransparentTower;
    private bool transparentTowerIsActive;
    private bool currentColor;
    
    
    

    public void ArcherButtonIsPressed()
    {
        archerButtonIsPressed = true;
    }

    // Start is called before the first frame update
    private void Start() {}

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out hit)) 
        {
            if (archerButtonIsPressed)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && currentColor)
                {
                    InstantiateTower(towerPrefab, hit);
                    Destroy(instantiatedTransparentTower); // Remove the transparent tower once a real one is placed
                    transparentTowerIsActive = false;
                }
                else if (!transparentTowerIsActive)
                {
                    transparentTowerIsActive = true;
                    instantiatedTransparentTower = InstantiateTransparentTower(transparentTower ,hit); // Only create it once when the player pressed a button
                }

                if(instantiatedTransparentTower != null && transparentTowerIsActive)
                {
                    // Move your transparent tower towards your mouse
                    var gridPos = SnapToGrid(hit.point, GridSize);
                    instantiatedTransparentTower.transform.position = gridPos;
                    
                    
                    GameObject tower = hit.transform.gameObject;
                    if (tower.CompareTag("Tower"))
                    {
                        ChangeColor(instantiatedTransparentTower, Color.red);
                        currentColor = false;
                    }
                    else if (tower.CompareTag("Ground"))
                    {
                        ChangeColor(instantiatedTransparentTower, Color.green);
                        currentColor = true;
                    }
                } 
            }
        }
    }

    private void InstantiateTower(GameObject tower, RaycastHit hit)
    {
        var gridPos = SnapToGrid(hit.point, GridSize);
        tower = Instantiate(tower, gridPos, Quaternion.identity);
    }

    private GameObject InstantiateTransparentTower(GameObject transparentTower, RaycastHit hit)
    {
        transparentTowerIsActive = true;
        var gridPos = SnapToGrid(hit.point, GridSize);
        // Make sure to return the instantiated object
        GameObject towerInstance = Instantiate(transparentTower, gridPos, Quaternion.identity);
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
        archerButtonIsPressed = true;
    }
    
    
    
    void ChangeColor(GameObject obj, Color newColor)
    {
        if (obj != null)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer != null && renderer.sharedMaterial != null)
            {
                renderer.sharedMaterial.color = newColor;
            }
        }
    }
}
