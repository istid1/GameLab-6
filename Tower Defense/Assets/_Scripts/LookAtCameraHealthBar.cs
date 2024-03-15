using Unity.Mathematics;
using UnityEngine;

namespace _Scripts
{
    public class LookAtCameraHealthBar : MonoBehaviour
    {

        [SerializeField] private Camera _mainCamera;
    
        // Start is called before the first frame update
        void Start()
        {
            GameObject cameraGameObject = GameObject.FindGameObjectWithTag("MainCamera");

            if (cameraGameObject != null)
            {
                _mainCamera = cameraGameObject.GetComponent<Camera>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
        }
    }
}
