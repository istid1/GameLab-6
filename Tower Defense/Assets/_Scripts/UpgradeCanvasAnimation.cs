using System;
using System.Collections;
using DG.Tweening;
using TMPro;
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
        [SerializeField] private RectTransform _sellButton;
        
        [SerializeField] private TowerController _towerController;
       
        private GameObject towerN, damageB, fireRateB, rangeB, xB, sellB;
        
        
        private void Start()
        {
            towerN = _towerName.gameObject;
            damageB = _damageButton.gameObject;
            fireRateB = _fireRateButton.gameObject;
            rangeB = _rangeButton.gameObject;
            xB = _xButton.gameObject;
            sellB = _sellButton.gameObject;
            MoveCanvasDisabled();
            //UpgradeCanvasDisabled();
        }

        private void Update()
        {
           
        }

        public void MoveCanvasActive()
        {
            UpgradeCanvasActive();
            
            _towerName.DOAnchorPos(new Vector2(0, 350), 0.5f);
            _damageButton.DOAnchorPos(new Vector2(150, 300), 0.5f);
            _fireRateButton.DOAnchorPos(new Vector2(150, 200), 0.5f);
            _rangeButton.DOAnchorPos(new Vector2(150, 100), 0.5f);
            _xButton.DOAnchorPos(new Vector2(250, 475), 0.5f);
            _sellButton.DOAnchorPos(new Vector2(150, -350), 0.5f);

        }
        public void MoveCanvasDisabled()
        {
            UpgradeCanvasDisabled();
            
            _towerName.DOAnchorPos(new Vector2(700, 0), 0.5f);
            _damageButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
            _fireRateButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
            _xButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
           _rangeButton.DOAnchorPos(new Vector2(700, 0), 0.5f);
           _sellButton.DOAnchorPos(new Vector2(700, 0), 0.5f);


        }

        
        public void UpgradeCanvasActive()
        {
            SetUpgradeCanvasState(true);
        }

            public void UpgradeCanvasDisabled()
        {
            if (this.gameObject.activeInHierarchy)
            {
                StartCoroutine(TurnOffCanvas());
            }
            
        }

        private void SetUpgradeCanvasState(bool isActive)
        {
            towerN.SetActive(isActive);
            damageB.SetActive(isActive);
            fireRateB.SetActive(isActive);
            rangeB.SetActive(isActive);
            xB.SetActive(isActive);
            sellB.SetActive(isActive);
        }
        
        
        IEnumerator TurnOffCanvas()
        {
            yield return new WaitForSeconds(0.4f);
            SetUpgradeCanvasState(false);
        }

        
    }
}
