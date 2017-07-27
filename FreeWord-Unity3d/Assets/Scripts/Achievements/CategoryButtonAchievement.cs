using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonAchievement : MonoBehaviour
{
    //Script for CategoryButton and his abilities


    /********************************* Variables *********************************/

    private string currentCatName;
    private Data data;
    private Dropdown dropDown;
    private Image wordImage;
    private List<Dropdown.OptionData> dropDownItems;
    private Dictionary<string, int> wordList;
    private List<string> pngUrlList;
    //private List<int> nbTimeWordDiscovered;
    private WWWForm form;
    private WWW w;


    /********************************* Loops *********************************/

    private void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        dropDown = GameObject.Find("WordDropdown").GetComponent<Dropdown>();
        wordImage = GameObject.Find("WordImage").GetComponent<Image>();
    }


    /********************************* Methods *********************************/

    
    public void SetDropDown()
    {
        StartCoroutine(SetWordList());
    }

    public IEnumerator SetWordList()
    {
        dropDownItems = new List<Dropdown.OptionData>();
        Color c = wordImage.color;
        Dictionary<string, int> tempWordList = new Dictionary<string, int>();
        wordList = new Dictionary<string, int>();
        pngUrlList = new List<string>();
        string[] tempString;
        List<string> newWords = new List<string>();
        currentCatName = this.transform.Find("Text_Up").GetComponent<Text>().text;

        //WB  request 
        form = new WWWForm();
        form.AddField("username", data.Username);
        form.AddField("language", data.LanguageForAchievement);
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

        dropDown.ClearOptions();
        if (wordList.Count != 0)
        {
            foreach (KeyValuePair<string, int> k in wordList.OrderByDescending(key => key.Value))
            {
                if (k.Value > 1)
                {
                    dropDownItems.Add(new Dropdown.OptionData(k.Key));
                }
                else if (k.Value == 1)
                {
                    newWords.Add(k.Key);

                }
            }

            //then write the new one at the end
            foreach (string s in newWords)
            {
                dropDownItems.Add(new Dropdown.OptionData(s));
            }

            //Set the image link to the word selected (done after webservice)
            c.a = 1;
            wordImage.color = c;

        }

        dropDown.AddOptions(dropDownItems);
    }

}
