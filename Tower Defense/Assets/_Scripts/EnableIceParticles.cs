using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class EnableIceParticles : MonoBehaviour
    {

        [SerializeField] private List<GameObject> _icicles;
    
        void Start()
        {
            Shuffle(_icicles);
            for(int i = 0; i < 30 && i < _icicles.Count; i++)
                _icicles[i].SetActive(true);
            
            // deactivate the rest of the icicles
            for (int i = 30; i < _icicles.Count; i++)
                _icicles[i].SetActive(false);
        }
        

        void Shuffle(List<GameObject> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = Random.Range(0, n--);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
