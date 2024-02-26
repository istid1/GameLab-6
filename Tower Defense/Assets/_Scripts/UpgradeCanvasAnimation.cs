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
            _towerName.DOAnchorPos(new Vector2(0, 500), 1f);
            _damageButton.DOAnchorPos(new Vector2(0, 400), 1f);
            _fireRateButton.DOAnchorPos(new Vector2(0, 300), 1f);
            _rangeButton.DOAnchorPos(new Vector2(0, 200), 1f);
            _xButton.DOAnchorPos(new Vector2(0, 600), 1f);
        }
        public void MoveCanvasDisabled()
        {
            _towerName.DOAnchorPos(new Vector2(555, 0), 1f);
                
            _damageButton.DOAnchorPos(new Vector2(555, 0), 1f);

            _fireRateButton.DOAnchorPos(new Vector2(555, 0), 1f);
            
            _xButton.DOAnchorPos(new Vector2(555, 0), 1f);

            _rangeButton.DOAnchorPos(new Vector2(555, 0), 1f)
                .OnComplete(() => _towerController._upgradeCanvas.SetActive(false));

        }

        
    }
}
