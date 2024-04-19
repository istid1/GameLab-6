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
        private int _previousRound = -1;
        
        
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
            
            if (_gameManager.currentRound % 2 == 0 && !_hasPlayed)   //Dissable canvases and start playing ads on round 2,4,6,8,etc... (not on round 0)
            {
                if (_gameManager.currentRound > 0)
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
            

            
            if (!_videoPlayer.isPlaying && _countDownComplete) // send the player back to the game if the video player is done playing and the skip button wasn't pressed.
            {
                ReturnToGame();
            }
            
        }
        
        private IEnumerator StartCountdown() //Countdown and show the player when they can press the skip ad button.
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

        private void ReturnToGame() //speaks for itself
        {
            _hasPlayed = false;
            _videoPlayer.Stop();
            Time.timeScale = 1;
            
            _canvas1.SetActive(true);
            _canvas2.SetActive(true);
            _skipButtonCanvas.SetActive(false);
            
        }
        
        

    }
}
