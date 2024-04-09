using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace _Scripts
{
    public class Ads : MonoBehaviour
    {

        [SerializeField] private GameManager _gameManager;
        [SerializeField] private VideoPlayer _videoPlayer;

        [SerializeField] private GameObject _canvas1;
        [SerializeField] private GameObject _canvas2;

        private bool _hasPlayed;
        public float _timeRemaining = 30;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

            if ( _gameManager.currentRound == 5 && !_hasPlayed)
            {
                _videoPlayer.Play();
                _hasPlayed = true;
                Time.timeScale = 0;
                _canvas1.SetActive(false);
                _canvas2.SetActive(false);
                StartCoroutine(StartCountdown());
            }
           
            
            
            
        }
        
        private IEnumerator StartCountdown()
        {
            while (_timeRemaining > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                _timeRemaining--;
            }
            _videoPlayer.Stop();
            Time.timeScale = 1;
            _canvas1.SetActive(true);
            _canvas2.SetActive(true);
            Debug.Log("DOOOOONEEEEEEE");
        }
        
        
        
    }
}
