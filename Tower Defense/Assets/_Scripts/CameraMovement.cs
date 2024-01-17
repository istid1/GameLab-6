
using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private bool _playerInput;
    [SerializeField] private bool topDownViewActive;
    [SerializeField] private float cycleLength = 1;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        CameraRotationInput();
    }
    

    private void CameraRotationInput()
 {
    if (_playerInput)
    {
        return;
    }

    if (Input.GetKeyDown(KeyCode.A) && !topDownViewActive)
    {
        RotateCamera(new Vector3(0, 90, 0));
    }
    else if (Input.GetKeyDown(KeyCode.D) && !topDownViewActive)
    {
        RotateCamera(new Vector3(0, -90, 0));
    }
    else if (Input.GetKeyDown(KeyCode.W) && !topDownViewActive)
    {
        topDownViewActive = true;
        RotateCamera(new Vector3(45, 0, 0));
    }
    else if (Input.GetKeyDown(KeyCode.S) && topDownViewActive)
    {
        topDownViewActive = false;
        RotateCamera(new Vector3(-45, 0, 0));
    }
 }


    private void RotateCamera(Vector3 rotationAngle)
    {
    _playerInput = true;
    transform.DOLocalRotate(rotationAngle, cycleLength, RotateMode.LocalAxisAdd)
              .OnComplete(() => _playerInput = false);
    
    }
   
}