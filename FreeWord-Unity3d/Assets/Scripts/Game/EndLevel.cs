using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

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
        def.GetComponent<Text>().text = "Not defined";
        back.GetComponent<Text>().text = "Back to category";
        next.GetComponent<Text>().text = "Next level";
        StartCoroutine(GameObject.Find("Image").GetComponent<ImageDownLoader>().LoadImage(data.MysteryWord, data.MysteryWordUrl, data.CurrentCatName));

        //Send list to WB 
        WWWForm form = new WWWForm();
        form.AddField("username", data.Username);
        form.AddField("language", data.LanguageToLearn);
        form.AddField("category", data.CurrentCatName);        
        form.AddField("wordsdiscovered",  JsonConvert.SerializeObject(data.GetWordListPlayed()));
        WWW w = new WWW(data.GetDbURL+"SendWordsDiscovered", form);
        yield return w;

        data.FinishLevel();
    }
}
    
