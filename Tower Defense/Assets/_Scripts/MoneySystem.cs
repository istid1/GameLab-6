using System;
using UnityEngine;
using TMPro;
namespace _Scripts
{
    public class MoneySystem : MonoBehaviour
    {

        [SerializeField] private TowerController _towerController;
        [SerializeField] private TMP_Text _moneyText;
        //[SerializeField] private EnemyHealth _enemyHealth;

        public static MoneySystem Instance { get; private set; }
        
        public int currentMoney;


        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of MoneySystem found!");
                return;
            }
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            currentMoney = 500;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(currentMoney);
            _moneyText.text = "Money : " + currentMoney;
        }


        public void DeductMoney(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount to deduct must be a non-negative value.");
            
            currentMoney -= amount;

            currentMoney = Mathf.Max(currentMoney, 0);
        }
    }
}
