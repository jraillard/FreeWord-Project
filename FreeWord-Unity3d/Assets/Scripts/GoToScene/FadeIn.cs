using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class FadeIn : MonoBehaviour
{
    public float fadeSpeed = 3f;
    private float alpha = 1.0f;

	// Use this for initialization
	void OnGUI()
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        Texture2D myTex;
        myTex = new Texture2D(1, 1);
        myTex.SetPixel(0, 0, Color.black);
        myTex.Apply();
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), myTex);
        alpha = Mathf.Lerp(alpha, -0.1f, fadeSpeed * Time.deltaTime);
    }

    public float BeginFadeIn()
    {
        return (fadeSpeed);
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
        BeginFadeIn();
    }
}
