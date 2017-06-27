using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour
{
    //Script for CategoryButton and his abilities


    /********************************* Variables *********************************/

    private GameObject catSelectedLabel1; //object for storing the two CategorySelected label
    private GameObject catSelectedLabel2;
    private GameObject wordLevelListLabel;
    private string currentCatName;
    private Data data;
    private List<string> wordList;
    private List<int> nbTimeWordDiscovered;


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

    public void SetCategorySelected_1()
    {
        wordList = new List<string>();
        nbTimeWordDiscovered = new List<int>();
        wordLevelListLabel.GetComponent<Text>().text = "\r\n";

        catSelectedLabel1.GetComponent<Text>().text = currentCatName + " : Level 0";
        data.GetWordListFromCategory(currentCatName, wordList, nbTimeWordDiscovered);
       
        for(int i=0; i<wordList.Count; i++)
        {
            wordLevelListLabel.GetComponent<Text>().text += "(" + nbTimeWordDiscovered[i] + ") " + wordList[i] + "\r\n";
        }

    }

    public void SetCategorySelected_2()
    {
        catSelectedLabel2.GetComponent<Text>().text = this.transform.Find("Text_Down").GetComponent<Text>().text;
    }
}
