using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToNextLevel : MonoBehaviour
{
    //Script to go to Game Scene

    /********************************* Variables *********************************/

    public bool start = false; // Variable for the beginning of the effect
    public float fadeDamp = 3f; // Duration of the effect
    public float alpha = 0.0f; // Opacity
    public bool isFadeIn = false; // Fade In or Out
    private bool effect = true;
    private GameObject obj;
    private string labelText;
    private Data data;
    private WWWForm form;
    private WWW w;

    /********************************* Methods ***********************************/

    // Start the effect
    public void Load()
    {
        GameObject init = new GameObject();
        init.name = "GoToGame";
        init.AddComponent<GoToGame>();
        GoToGame scr = init.GetComponent<GoToGame>();
        StartCoroutine(RefreshWordListFromCategory());
        scr.start = true;
    }

    public IEnumerator RefreshWordListFromCategory()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        //Load the new list with updated nbtime values

        print(data.Username);
        form = new WWWForm();
        form.AddField("username", data.Username);
        form.AddField("language", data.LanguageToLearn);
        form.AddField("category", data.CurrentCatName);
        w = new WWW(data.GetDbURL+"GetWordsInCategory", form);
        yield return w;

        Dictionary<string, int> tempD = new Dictionary<string, int>();
        Dictionary<string, int> words = new Dictionary<string, int>();
        List<string> urls = new List<string>();
        string[] tempS;

        tempD = JsonConvert.DeserializeObject<Dictionary<string, int>>(w.text);

        foreach (KeyValuePair<string, int> k in tempD)
        {
            tempS = k.Key.Split('|');
            words.Add(tempS[0], k.Value);
            urls.Add(tempS[1]);
        }

        //send lists to DataObject
        data.SetWordListFromCategory(words, urls);
    }

    void OnGUI()
    {
        if (!start) { return; }
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
        if (alpha >= 1 && !isFadeIn && effect == true)
        {
            SceneManager.LoadSceneAsync("Game");
            DontDestroyOnLoad(gameObject);
            effect = false;
            // Fade Out
        }
        else if (alpha <= 0 && isFadeIn)
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
