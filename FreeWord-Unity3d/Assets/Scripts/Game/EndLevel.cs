using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndLevel : MonoBehaviour {

    public Animator FC;
    public GameObject congrats;
    public GameObject word;
    public GameObject newword;
    public GameObject def;
    public GameObject back;
    public GameObject next;
    private Data data;

    void Start()
    {
        StartCoroutine(Time());
    }

    IEnumerator Time() {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        yield return new WaitForSeconds(5);
        congrats.GetComponent<Text>().text = "Congratulations !";
        newword.GetComponent<Text>().text = "You unlock the new word:";
        word.GetComponent<Text>().text += data.MysteryWord;
        /*test
          word.GetComponent<Text>().text += "\r\n"+data.MysteryWord + "\r\n";
        
        foreach(string s in data.wordsDiscoveredDuringGame)
        {
            word.GetComponent<Text>().text += s + "\r\n";
        }
        */
        def.GetComponent<Text>().text = "Not defined";
        back.GetComponent<Text>().text = "Back to category";
        next.GetComponent<Text>().text = "Next level";
        data.FinishLevel();
    }
}
    
