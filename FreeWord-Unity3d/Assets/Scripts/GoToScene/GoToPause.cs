using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToPause : MonoBehaviour {
    // Start the effect
    public void Load()
    {
        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
    }

	
    public void Resume()
    {
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("Pause");
    }

    public void Level()
    {
        Time.timeScale = 1.0f;
        GameObject.Find("Main Camera").GetComponent<GoToLevelSelectionbyPause>().Load();
    }
}
