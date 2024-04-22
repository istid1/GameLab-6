using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        public bool zoomIsActive;
        public bool _playerInput;
        [SerializeField] private bool topDownViewActive;
        public float cycleLength = 0.5f;
        private float _targetFOV;
        private IEnumerator fovCoroutine;
        [SerializeField] private Camera _cameraFOV;
        private const float zoomSpeed = 0.25f;

        // Update is called once per frame
        void FixedUpdate()
        {
            CameraRotationInput();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (fovCoroutine != null)
                {
                    StopCoroutine(fovCoroutine);
                }

                if (!zoomIsActive)
                {
                    _targetFOV = 50f;
                    zoomIsActive = true;
                }
                else
                {
                    _targetFOV = 75;
                    zoomIsActive = false;
                }
      
                fovCoroutine = LerpFieldOfView(_cameraFOV, _targetFOV, zoomSpeed);
                StartCoroutine(fovCoroutine);
            }
        }
        
        IEnumerator LerpFieldOfView(Camera camera, float target, float duration)
        {
            float elapsedTime = 0;
            float startFOV = camera.fieldOfView;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                camera.fieldOfView = Mathf.Lerp(startFOV, target, elapsedTime / duration);
                yield return null;
            }
            camera.fieldOfView = target;
        }

        private void Start()
        {
            zoomIsActive = false;
            _cameraFOV = GetComponentInChildren<Camera>();
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
            transform.DOLocalRotate(rotationAngle, cycleLength, RotateMode.LocalAxisAdd).SetEase(Ease.Flash)
                .OnComplete(() => _playerInput = false);
        }
    }
}