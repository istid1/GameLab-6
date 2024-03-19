using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class IceTowerAnimation : MonoBehaviour
    {

        [SerializeField] private TowerVariables _towerVariables;
        [SerializeField] private GameObject _projectile;


        private int _currentLevel;        
        private float _startScale = 0.2f;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

            _currentLevel = _towerVariables._currentRangeUpgradeLevel;
            
            Debug.Log(_currentLevel);
            
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
            
            switch (_towerVariables._currentRangeUpgradeLevel)
            {
                case 0:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.1f, 0.5f, _startScale + 0.1f), 0.5f);
                    break;
                case 1:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.2f, 0.6f, _startScale + 0.2f), 0.5f);
                    break;
                case 2:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.3f, 0.7f, _startScale + 0.3f), 0.5f);
                    break;
                case 3:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.4f, 0.8f, _startScale + 0.4f), 0.5f);
                    break;
                case 4:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.5f, 0.9f, _startScale + 0.5f), 0.5f);
                    break;
                case 5:
                    _projectile.transform.DOScale(new Vector3(_startScale + 0.6f, 1f, _startScale + 0.6f), 0.5f);
                    break;
            }
        }


        private void DownScale()
        {
            _projectile.transform.DOScale(new Vector3(_startScale, 0.5f, _startScale), 0.5f);
        }
    
    }
}
