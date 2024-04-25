using System;
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
                _evenHasBeenRecorded = true; //// This doesn't work
            }
        }

        public void AdButtonPress()
        {
        

            // The ‘levelCompleted’ event will get cached locally
            //and sent during the next scheduled upload, within 1 minute
            //AnalyticsService.Instance.CustomData("AdButtonPress");
            AnalyticsService.Instance.RecordEvent("AdButtonPress");
            
            // You can call Events.Flush() to send the event immediately
            AnalyticsService.Instance.Flush();
        }
    
    
        public void GiveConsent()       //allow Data Collection
        {
            AnalyticsService.Instance.StartDataCollection();

            Debug.Log($"Consent has been provided. The SDK is now collecting data!");
        }
    }
}
