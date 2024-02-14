using UnityEngine;

namespace _Scripts
{
    public class TowerID : MonoBehaviour
    {
        public int ID { get; private set; } 
        public TowerID(int id)
        {
            ID = id;
        }
    }
}
