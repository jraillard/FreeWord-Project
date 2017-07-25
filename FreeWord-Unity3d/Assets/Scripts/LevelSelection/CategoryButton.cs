using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;

public class CategoryButton : MonoBehaviour
{
    //Script for CategoryButton and his abilities


    /********************************* Variables *********************************/

    private GameObject catSelectedLabel1; //object for storing the two CategorySelected label
    private GameObject catSelectedLabel2;
    private GameObject wordLevelListLabel;
    private string currentCatName;
    private Data data;
    private Dictionary<string, int> wordList;
    private List<string> pngUrlList;
    private WWWForm form;
    private WWW w;


    /********************************* Loops *********************************/

    private void Start()
    {
        catSelectedLabel1 = GameObject.Find("CategorySelected_1");
        catSelectedLabel2 = GameObject.Find("CategorySelected_2");
        wordLevelListLabel = GameObject.Find("WordsInCategory");
        currentCatName = this.transform.Find("Text_Up").GetComponent<Text>().text;
        data = GameObject.Find("DataObject").GetComponent<Data>();

    }


    /********************************* Methods *********************************/

    public void SetCategorySelected()
    {
       StartCoroutine(SetAllLabels());
    }

    public IEnumerator SetAllLabels()
    {
        int currentLevel;
        int nbWordDiscovered;
        wordLevelListLabel.GetComponent<Text>().text = "\r\n";
        List<string> newWords = new List<string>();

        Dictionary<string, int> tempWordList = new Dictionary<string, int>();
        wordList = new Dictionary<string, int>();
        pngUrlList = new List<string>();

        string[] tempString;

        //WB  request 
        form = new WWWForm();
        form.AddField("username", data.Username);
        form.AddField("language", data.LanguageToLearn);
        form.AddField("category", currentCatName);
        w = new WWW(data.GetDbURL+"GetWordsInCategory", form);
        yield return w;

        tempWordList = JsonConvert.DeserializeObject<Dictionary<string, int>>(w.text);

        //implement the list
        foreach (KeyValuePair<string, int> k in tempWordList)
        {
            tempString = k.Key.Split('|');
            wordList.Add(tempString[0], k.Value);
            pngUrlList.Add(tempString[1]);
        }

        //send lists to DataObject
        data.SetWordListFromCategory(wordList, pngUrlList);
        data.CurrentCatName = currentCatName;


        //Set list label
        if (wordList.Count == 0)
        {
            wordLevelListLabel.GetComponent<Text>().text = "No Words in this category\r\n";
        }
        else
        {
            //first write the words discovered ordered by there value
           // wordList.OrderBy(x => x.Value);

            foreach (KeyValuePair<string, int> k in wordList.OrderByDescending(key => key.Value))
            {
                if(k.Value > 1)
                {
                    wordLevelListLabel.GetComponent<Text>().text +=  (k.Value - 1) + " | " + k.Key + "\r\n";
                }else if(k.Value == 1)
                {
                    newWords.Add(k.Key);
                    
                }                                   
            }

            //then write the new one
            foreach(string s in newWords)
            {
                wordLevelListLabel.GetComponent<Text>().text += "New | " + s + "\r\n";
            }
        }
        
        //Set selected label
        nbWordDiscovered = wordList.Where(d => d.Value > 0).Count();

        foreach (KeyValuePair<string, int> k in wordList)
        {
            //print(k.Key + "|" + k.Value);
        }

        if (nbWordDiscovered == 0)
        {
            currentLevel = 0;
            data.CurrentLevel = 0;
        }
        else
        {
            //mean you already make one time the level0 where you discover 5word + 1 mystery
            currentLevel = nbWordDiscovered - 5;
            data.CurrentLevel = currentLevel;
        }


        if (data.LanguageToPlay == "English") { catSelectedLabel1.GetComponent<Text>().text = currentCatName + " : Level " + currentLevel + "/" + wordList.Count; }
        else { catSelectedLabel1.GetComponent<Text>().text = currentCatName + " : Niveau " + currentLevel + "/" + wordList.Count; }
        catSelectedLabel2.GetComponent<Text>().text = this.transform.Find("Text_Down").GetComponent<Text>().text;
        
    }

}
