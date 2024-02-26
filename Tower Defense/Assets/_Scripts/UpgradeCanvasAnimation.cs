using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public class UpgradeCanvasAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _towerName;
        [SerializeField] private RectTransform _damageButton;
        [SerializeField] private RectTransform _fireRateButton;
        [SerializeField] private RectTransform _rangeButton;
        [SerializeField] private RectTransform _xButton;
       
        
        [SerializeField] private TowerController _towerController;

        public void MoveCanvasActive()
        {
            _towerName.DOAnchorPos(new Vector2(100, 350), 0.5f);
            _damageButton.DOAnchorPos(new Vector2(250, 300), 0.5f);
            _fireRateButton.DOAnchorPos(new Vector2(250, 250), 0.5f);
            _rangeButton.DOAnchorPos(new Vector2(250, 200), 0.5f);
            _xButton.DOAnchorPos(new Vector2(300, 400), 0.5f);
          
            
        }
        public void MoveCanvasDisabled()
        {
            _towerName.DOAnchorPos(new Vector2(700, 0), 0.5f);
            _damageButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
            _fireRateButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
            _xButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
           _rangeButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
                

        }

        
    }
}
