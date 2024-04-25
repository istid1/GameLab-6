using System;
using System.Collections;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace _Scripts
{
    public class AnalyticsPinger : MonoBehaviour
    {
        [SerializeField] private Ads _ads;
        private bool _evenHasBeenRecorded = false;
        
        // Start is called before the first frame update
        async void Start()
        {
            
            try
            {
                await UnityServices.InitializeAsync();
                GiveConsent();
            }
            catch (ConsentCheckException e)
            {
                Debug.Log(e.ToString());
            }
        
        }


        private void Update()
        {
            if (_ads.FullAdHasBeenWatched && !_evenHasBeenRecorded)
            {
                AnalyticsService.Instance.RecordEvent("AdFullWatch");
                AnalyticsService.Instance.Flush();
                _evenHasBeenRecorded = true;
                StartCoroutine(ReturnToGameDelay(1f)); //return to the game with a delay to make sure the analytics got pinged
            }
        }

        
        private IEnumerator ReturnToGameDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _evenHasBeenRecorded = false;
            _ads.ReturnToGame();
        }
        
        public void AdButtonPress()
        {
            
            if (!_evenHasBeenRecorded)
            {
                AnalyticsService.Instance.RecordEvent("AdButtonPress");
                AnalyticsService.Instance.Flush(); //send the event to the cloud instantly
            }
            
            
        }
    
    
        public void GiveConsent()       //allow Data Collection
        {
            AnalyticsService.Instance.StartDataCollection();

            Debug.Log($"Consent has been provided. The SDK is now collecting data!");
        }
    }
}
