using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class IceTowerAnimation : MonoBehaviour
    {

        [SerializeField] private TowerVariables _towerVariables;
        [SerializeField] private GameObject _projectile;

        public bool isAttacking = false;
        // The radius of the sphere
        private float _radius = 3.0f;
        private bool _enemyInRange = false;
        
        // The maximum distance the spherecast will check
        private float _maxDistance = 0f;
        
        private int _currentLevel;        
        private float _startScale = 0.15f;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

            _currentLevel = _towerVariables._currentRangeUpgradeLevel;
            
            RadiusScaleWithRange();
            SphereCastAndLog();
            
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
            switch (_towerVariables._currentRangeUpgradeLevel)
            {
                case 0:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.15f, 0.1f, _startScale + 0.15f), 0.5f);
                    break;
                case 1:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.2f, 0.15f, _startScale + 0.2f), 0.5f);
                    break;
                case 2:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.25f, 0.2f, _startScale + 0.25f), 0.5f);
                    break;
                case 3:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.3f, 0.25f, _startScale + 0.3f), 0.5f);
                    break;
                case 4:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.35f, 0.3f, _startScale + 0.35f), 0.5f);
                    break;
                case 5:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.375f, 0.325f, _startScale + 0.375f), 0.5f);
                    break;
            }
        }


        private void DownScale()
        {
            isAttacking = false;
            _projectile.transform.DOScale(new Vector3(_startScale, 0.5f, _startScale), 0.5f);
        }
    
        
        
        
        private void SphereCastAndLog()
        {
            var transform1 = transform;
            var ray = new Ray(transform1.position, transform1.forward);
            RaycastHit[] hits;

            var layerMask = 1 << LayerMask.NameToLayer("Enemy");

            
           
            hits = Physics.SphereCastAll(ray, _radius, layerMask);

            foreach (var hit in hits)
            {
                
                if(hit.collider.gameObject.CompareTag("Enemy"))
                {
                    _enemyInRange = true;
                   UpScale();
                   break;
                }
                if(!_enemyInRange)
                {
                    DownScale();
                }
            }
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
