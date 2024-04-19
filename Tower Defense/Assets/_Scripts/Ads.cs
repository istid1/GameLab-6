using System.Collections;
using UnityEngine;
using UnityEngine.Video;

using UnityEngine.UI;

namespace _Scripts
{
    public class Ads : MonoBehaviour
    {

        [SerializeField] private GameManager _gameManager;
        [SerializeField] private VideoPlayer _videoPlayer;

        [SerializeField] private GameObject _canvas1;
        [SerializeField] private GameObject _canvas2;
        [SerializeField] private GameObject _skipButtonCanvas;

        [SerializeField] private GameObject _exitAdButton;

        public bool _hasPlayed;
        public float _timeRemaining = 30;

        
        public float totalAdTime = 30;
        private bool _countDownComplete;
        
        [SerializeField] private Image countdownCircle;
        
        // Start is called before the first frame update
        void Start()
        {
            countdownCircle = countdownCircle.GetComponent<Image>();
            countdownCircle.fillAmount = 1;
        }

        // Update is called once per frame
        void Update()
        {

            if ( _gameManager.currentRound % 2 == 0 && !_hasPlayed)
            {
                _skipButtonCanvas.SetActive(true);
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
                countdownCircle.fillAmount = _timeRemaining / totalAdTime;
                if (countdownCircle.fillAmount == 0)
                {
                    
                    countdownCircle.enabled = false;
                    _exitAdButton.SetActive(true);
                }
            }

            _countDownComplete = true;
           
           
        }

        public void ExitAdButton()
        {
           ReturnToGame();
        }

        private void ReturnToGame()
        {
            _videoPlayer.Stop();
            Time.timeScale = 1;
            
            _canvas1.SetActive(true);
            _canvas2.SetActive(true);
            _skipButtonCanvas.SetActive(false);
        }
        
        

    }
}
