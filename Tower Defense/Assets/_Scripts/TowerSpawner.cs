
                // TowerSpawner.cs
using UnityEngine;
public class TowerSpawner : MonoBehaviour
{
    public Camera mainCam;
    private const string GroundTag = "Ground";
    private const string TowerTag = "Tower";
    public GameObject towerPrefab;
    void Update()
    {
       
        
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                CheckAndInstantiateTower(hit);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            DestroyTower();
        }
        
    }

    private void CheckAndInstantiateTower(RaycastHit hit)
    {
        if (IsGround(hit.collider))
        {
            Instantiate(towerPrefab, hit.point, Quaternion.identity);
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
}
            
        
    

