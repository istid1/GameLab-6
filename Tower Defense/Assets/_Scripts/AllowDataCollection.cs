using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllowDataCollection : MonoBehaviour
{
  

    public void AllowDataCollectionButton()
    {
        SceneManager.LoadScene(1);
    }

    public void DissallowDataCollectionButton()
    {
        Application.Quit();
    }
    
}
