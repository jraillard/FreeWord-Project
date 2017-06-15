using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToGame : MonoBehaviour {
     
    //Script to go to Game Scene

    /********************************* Variables *********************************/

    private GameObject obj;
    private string labelText;

    /********************************* Loops *********************************/

    private void Start()
    {
        obj = GameObject.Find("WordsInCategory");
    }

    /********************************* Methods *********************************/

    public void PlayGame()
    {
        labelText = obj.GetComponent<Text>().text;

        if (labelText != "" && labelText != "\r\n(0) No Words in this category\r\n") //a category is selected (first cateogry character = capital letter)
        {
           SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }
        
    }
}
