using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts
{
    public class IceTowerAnimation : MonoBehaviour
    {

        [SerializeField] private TowerVariables _towerVariables;
        [SerializeField] private GameObject _projectile;

        public bool isAttacking;
        // The radius of the sphere
        private float _radius = 3.0f;
        private bool _enemyInRange;
        
        
        private float _maxDistance = 0f;
        
        private int _currentLevel;        
        private float _startScale = 0.15f;
        

        // Update is called once per frame
        private void Update()
        {

            _currentLevel = _towerVariables._currentRangeUpgradeLevel;
            
            RadiusScaleWithRange();
            SphereCast();
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                UpScale();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                DownScale();
            }
            
        }

        private void UpScale()
        {
            isAttacking = true;
            var scaleIncrement = _currentLevel * 0.025f + _startScale;
            var scaleY = _currentLevel * 0.025f;

            if (_currentLevel == 5)
            {
                scaleIncrement += 0.025f;
                scaleY += 0.025f;
            }

            _projectile.transform.DOScale(new Vector3(scaleIncrement, scaleY, scaleIncrement), 0.5f);
        }


        private void DownScale()
        {
            isAttacking = false;
            _projectile.transform.DOScale(new Vector3(_startScale, 0.5f, _startScale), 0.5f);
        }
    
        
        private void SphereCast()
        {
            var transform1 = transform;
            var ray = new Ray(transform1.position, transform1.forward);
            RaycastHit[] hits;
            var layerMask = 1 << LayerMask.NameToLayer("Enemy");
            hits = Physics.SphereCastAll(ray, _radius, _maxDistance, layerMask);
            _enemyInRange = false;
            
            //Declare list to store components
             var enemyComponentsInRange = new List<NavMeshAgent>(); 

            if (hits.Any(hit => hit.collider.gameObject.CompareTag("Enemy")))
            {
                foreach(var hitResult in hits.Where(hit => hit.collider.gameObject.CompareTag("Enemy")))
                {
                    NavMeshAgent component = hitResult.collider.gameObject.GetComponent<NavMeshAgent>();
                    if (component != null)
                    {
                        //add component to the list
                        enemyComponentsInRange.Add(component);
                        component.speed /= 2;
                    }
                }
                _enemyInRange = true;
                UpScale();
                return;
            }

            //No enemies in range
            DownScale();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var transform1 = transform;
            var position = transform1.position;
            var forward = transform1.forward;
            Gizmos.DrawRay(position, forward * _maxDistance);
            Gizmos.DrawWireSphere(position + forward * _maxDistance, _radius);
        }

        private void RadiusScaleWithRange()
        {
            _radius = _currentLevel switch
            {
                0 => 5f,
                1 => 5.5f,
                2 => 6f,
                3 => 6.5f,
                4 => 7f,
                5 => 7.25f,
                _ => _radius
            };
        }
        
    }
}
