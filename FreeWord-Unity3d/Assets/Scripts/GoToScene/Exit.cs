using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Exit : MonoBehaviour {

    private Data data;

    private void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
    }
    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        data.SetVerifExit = false;
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("Exit");
    }
}
