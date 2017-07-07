using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLevelSelection : MonoBehaviour {

    /********************************* Variables *********************************/

    public bool start = false; // Variable for the beginning of the effect
    public float fadeDamp = 3f; // Duration of the effect
    public float alpha = 0.0f; // Opacity
    public bool isFadeIn = false; // Fade In or Out
    private bool effect = true;
    private Data data;

    /********************************* Main Events *******************************/

    private void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
    }
    // Start the effect

    public void LoadFromGame()
    {
        GameObject init = new GameObject();
        init.name = "GoToLevelSelection";
        init.AddComponent<GoToLevelSelection>();
        GoToLevelSelection scr = init.GetComponent<GoToLevelSelection>();
        scr.start = true;
    }

    public void LoadEN()
    {
        data.LanguageToLearn = "English";
        GameObject init = new GameObject();
        init.name = "GoToLevelSelection";
        init.AddComponent<GoToLevelSelection>();
        GoToLevelSelection scr = init.GetComponent<GoToLevelSelection>();
        scr.start = true;
    }

    public void LoadFR()
    {
        data.LanguageToLearn = "Français";
        GameObject init = new GameObject();
        init.name = "GoToLevelSelection";
        init.AddComponent<GoToLevelSelection>();
        GoToLevelSelection scr = init.GetComponent<GoToLevelSelection>();
        scr.start = true;
    }

	void OnGUI () {
        if (!start) {return;}
        // Create the texture (one pixel)
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        Texture2D myTex;
        myTex = new Texture2D(1, 1);
        myTex.SetPixel(0, 0, Color.black);
        myTex.Apply();

        // Repeat the pixel on the whole screen
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myTex);
        if (isFadeIn) { alpha = Mathf.Lerp(alpha, -0.1f, fadeDamp * Time.deltaTime); }
        else { alpha = Mathf.Lerp(alpha, 1.1f, fadeDamp * Time.deltaTime); } 
        
        // Fade In
        if(alpha >= 1 && !isFadeIn && effect==true)
        {
            SceneManager.LoadSceneAsync("LevelSelection");
            DontDestroyOnLoad(gameObject);
            effect = false;
        // Fade Out
        }else if(alpha<=0 && isFadeIn)
        {
            Destroy(gameObject);
        }
	}

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnNextSceneLoaded;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnNextSceneLoaded;
    }

    void OnNextSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isFadeIn = true;
    }
}
