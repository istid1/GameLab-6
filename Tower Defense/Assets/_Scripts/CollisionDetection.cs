using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public Renderer transparentTowerRenderer;

    public bool currentColor;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("currentColor is: " + currentColor);
        // Ensuring transparentTowerRenderer has a reference.
        transparentTowerRenderer = GetComponent<Renderer>();
        if (transparentTowerRenderer == null)
        {
            Debug.Log("No Renderer component found.");
        }
        else if (transparentTowerRenderer.sharedMaterial == null)
        {
            Debug.Log("No Material found on Renderer.");
        }
        
        ChangeColor(this.gameObject, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionStay(Collision other)
    {
        // Debug.Log("Collision detected with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Tower"))
        {
            Debug.Log("I hit another Tower");
            
           
            ChangeColor(this.gameObject, Color.red);
           
            currentColor = false;
        }
       
    }
    
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            ChangeColor(this.gameObject, Color.green);
            currentColor = true;
        }
    }
    
    
    void ChangeColor(GameObject obj, Color newColor)
    {
        if (obj != null)
        {
            if (transparentTowerRenderer != null && transparentTowerRenderer.material != null)
            {
                transparentTowerRenderer.material.color = newColor;
            }
        }
    }
    
}
