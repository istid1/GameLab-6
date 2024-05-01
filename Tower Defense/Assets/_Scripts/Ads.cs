using System.Collections;
using System.Collections.Generic;

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

        [SerializeField] private AnalyticsPinger _analyticsPinger;
        [SerializeField] private GameObject _exitAdButton;

        [SerializeField] private AudioSource _audioSource;
        
        //Random ad system
        public List<VideoClip> adList;
        private VideoClip randomClip;

        private int _lastRound = -1;

        public bool FullAdHasBeenWatched = false;
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
         private void  Update()
        {
            
            if (_gameManager.currentRound % 2 == 0 && _lastRound != _gameManager.currentRound)
            {
                if (_gameManager.currentRound > 0)
                {
                    _lastRound = _gameManager.currentRound;
                    _skipButtonCanvas.SetActive(true);
                    RandomClipSelector();
                    _videoPlayer.clip = randomClip;
                    _audioSource.volume = 0f;
                    _videoPlayer.Play();

                    //_hasPlayed = true;
                    Time.timeScale = 0;
                    _canvas1.SetActive(false);
                    _canvas2.SetActive(false);
                    StartCoroutine(StartCountdown());
                }
                
            }

            if (!_videoPlayer.isPlaying && _countDownComplete)
            {
                FullAdHasBeenWatched = true;
                
            }
            
        }

       
        private IEnumerator StartCountdown() //Countdown and show the player when they can press the skip ad button.
        {
            FullAdHasBeenWatched = false;
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
        
        
        

        public void ReturnToGame() //speaks for itself
        {
            
            _videoPlayer.Stop();
            _audioSource.volume = 0.01f;
            Time.timeScale = 1;
            
            _canvas1.SetActive(true);
            _canvas2.SetActive(true);
            countdownCircle.enabled = true;
            countdownCircle.fillAmount = totalAdTime;
            _exitAdButton.SetActive(false);
            _skipButtonCanvas.SetActive(false);
            _countDownComplete = false;
            _timeRemaining = 5f;
            FullAdHasBeenWatched = false;

        }
        
       


        private void RandomClipSelector()
        {

            int randomIndex = Random.Range(0, adList.Count);
            randomClip = adList[randomIndex];

        }

    }
}
