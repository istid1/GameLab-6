using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsPinger : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void AdButtonPress()
    {
      
        int currentLevel = Random.Range(1, 4); // Gets a random number from 1-3

        // Define Custom Parameters
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "levelName", "level" + currentLevel.ToString()}
        };

        // The ‘levelCompleted’ event will get cached locally
        //and sent during the next scheduled upload, within 1 minute
        AnalyticsService.Instance.CustomData("AdButtonPress", parameters);

        // You can call Events.Flush() to send the event immediately
        AnalyticsService.Instance.Flush();
    }
    
    
    public void GiveConsent()       //allow Data Collection
    {
        AnalyticsService.Instance.StartDataCollection();

        Debug.Log($"Consent has been provided. The SDK is now collecting data!");
    }
}