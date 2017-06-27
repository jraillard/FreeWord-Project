using System.Collections;
using System.Collections.Generic;
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
    private List<string> wordList;
    private List<int> nbTimeWordDiscovered;


    /********************************* Loops *********************************/

    private void Start()
    {
        currentCatName = this.transform.Find("Text_Up").GetComponent<Text>().text;
        data = GameObject.Find("DataObject").GetComponent<Data>();
        dropDown = GameObject.Find("WordDropdown").GetComponent<Dropdown>();
        wordImage = GameObject.Find("WordImage").GetComponent<Image>();
    }


    /********************************* Methods *********************************/

    public void SetWordDropDownList()
    {
        wordList = new List<string>();
        nbTimeWordDiscovered = new List<int>();
        dropDownItems = new List<Dropdown.OptionData>();
        Color c = wordImage.color;
        dropDown.ClearOptions();

        data.GetWordListFromCategory(currentCatName, wordList, nbTimeWordDiscovered);

        if(wordList[0]=="No Words in this category")
        {
            //Don't show Image
            c.a = 0;
            wordImage.color = c;

            //Add one option 
            dropDownItems.Add(new Dropdown.OptionData(wordList[0]));
        }
        else
        {
            //Add options 
            for (int i = 0; i < wordList.Count; i++)
            {
                dropDownItems.Add(new Dropdown.OptionData("(" + nbTimeWordDiscovered[i] + ") " + wordList[i]));
            }

            //Set the image link to the word selected (done after webservice)
            c.a = 1;
            wordImage.color = c;

        }

        


        dropDown.AddOptions(dropDownItems);
    }

}
