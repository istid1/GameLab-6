using UnityEngine;

namespace _Scripts
{
    public class CollisionDetection : MonoBehaviour
    {
        public Renderer transparentTowerRenderer;
        
        // Start is called before the first frame update
        void Start()
        {
            // Ensuring transparentTowerRenderer has a reference.
            transparentTowerRenderer = GetComponent<Renderer>();
        
            ChangeColor(this.gameObject, Color.green);
        }
    
        private void OnCollisionStay(Collision other)
        {
            // Debug.Log("Collision detected with " + other.gameObject.name);
            if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("UpgradeTag"))
            {
                ChangeColor(this.gameObject, Color.red);
            }
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("UpgradeTag"))
            {
                ChangeColor(this.gameObject, Color.green);
            }
        }
    
    
        void ChangeColor(GameObject obj, Color newColor)
        {
            if (obj == null) return;
            if (transparentTowerRenderer != null && transparentTowerRenderer.material != null)
            {
                transparentTowerRenderer.material.color = newColor;
            }
        }
    
    }
}
