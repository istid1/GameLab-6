using UnityEngine;
namespace _Scripts
{
    public class TowerTypeing : MonoBehaviour
    {
    
        [SerializeField] private TowerType towerType;
        [HideInInspector] public string _towerTypeString;
        // Start is called before the first frame update
        void Start()
        {
            SetTowerType();
        }
        
        public enum TowerType
        {
            BallistaTower,
            FireTower,
            IceTower,
            LightningTower,
            BombTower
        }
    
        private void SetTowerType()
        {
            if (towerType == TowerType.BallistaTower)
            {
                _towerTypeString = "BallistaTower";
            }
            if (towerType == TowerType.FireTower)
            {
                _towerTypeString = "FireTower";
            }
            if (towerType == TowerType.IceTower)
            {
                _towerTypeString = "IceTower";
            }
            if (towerType == TowerType.LightningTower)
            {
                _towerTypeString = "LightningTower";
            }
            if (towerType == TowerType.BombTower)
            {
                _towerTypeString = "BombTower";
            }
        }
    }
}
