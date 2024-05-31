using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    
        public void SwitchScene()
        {
            
            // get the active scene's build index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // check if Scene0 (index 0) is active
            if (currentSceneIndex == 0)
            {
                // Load  Scene1 (index 1)
                SceneManager.LoadScene(1);
            }
            else if (currentSceneIndex == 1)
            {
                // Load Scene0 (index 0)
                SceneManager.LoadScene(0);
            }
        }
    
}
    
