using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LngButton: MonoBehaviour
{
    /******************************* Variables *******************************/

    private Data data;
    private ChoiceCategoryManagementAchievement catM;
    private List<Button> catButtonList;
    private string tempS = "";
    private Color c;
    private Dropdown dropDown;
    private Image imgDownLoad;

    /*****************************  Main Events ******************************/

    void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        catM = this.GetComponent<ChoiceCategoryManagementAchievement>();
        dropDown = GameObject.Find("WordDropdown").GetComponent<Dropdown>();
        imgDownLoad = GameObject.Find("WordImage").GetComponent<Image>();
    }

    /******************************** Methods *********************************/

    public void SetCategoryInEN()
    {
        catButtonList = catM.GetCatButtonList();

        if(data.LanguageForAchievement != "English") //don't do if EN already
        {
            if(data.LanguageToPlay == "English") //lng to achieve = lng to play
            {
                foreach (Button b in catButtonList)
                {
                    //switch
                    tempS = b.transform.Find("Text_Up").GetComponent<Text>().text;
                    b.transform.Find("Text_Up").GetComponent<Text>().text = b.transform.Find("Text_Down").GetComponent<Text>().text;
                    b.transform.Find("Text_Down").GetComponent<Text>().text = tempS;
                    //make the second text unvisible
                    c = b.transform.Find("Text_Down").GetComponent<Text>().color;
                    c.a = 0;
                    b.transform.Find("Text_Down").GetComponent<Text>().color = c;

                }
            }else
            {
                foreach (Button b in catButtonList)
                {
                    //switch
                    tempS = b.transform.Find("Text_Up").GetComponent<Text>().text;
                    b.transform.Find("Text_Up").GetComponent<Text>().text = b.transform.Find("Text_Down").GetComponent<Text>().text;
                    b.transform.Find("Text_Down").GetComponent<Text>().text = tempS;
                    //make the second text unvisible
                    c = b.transform.Find("Text_Down").GetComponent<Text>().color;
                    c.a = 1;
                    b.transform.Find("Text_Down").GetComponent<Text>().color = c;
                }
            }

            data.LanguageForAchievement = "English";
            dropDown.ClearOptions();
            //don't show image  ~= clear image
            c = imgDownLoad.color;
            c.a = 0;
            imgDownLoad.color = c;
        }
    }

    public void SetCategoryInFR()
    {
        catButtonList = catM.GetCatButtonList();

        if (data.LanguageForAchievement != "Français") //don't do if EN already
        {
            if (data.LanguageToPlay == "Français") //lng to achieve = lng to play
            {
                foreach (Button b in catButtonList)
                {
                    //switch
                    tempS = b.transform.Find("Text_Up").GetComponent<Text>().text;
                    b.transform.Find("Text_Up").GetComponent<Text>().text = b.transform.Find("Text_Down").GetComponent<Text>().text;
                    b.transform.Find("Text_Down").GetComponent<Text>().text = tempS;
                    //make the second text unvisible
                    c = b.transform.Find("Text_Down").GetComponent<Text>().color;
                    c.a = 0;
                    b.transform.Find("Text_Down").GetComponent<Text>().color = c;
                }
            }
            else
            {
                foreach (Button b in catButtonList)
                {
                    //switch
                    tempS = b.transform.Find("Text_Up").GetComponent<Text>().text;
                    b.transform.Find("Text_Up").GetComponent<Text>().text = b.transform.Find("Text_Down").GetComponent<Text>().text;
                    b.transform.Find("Text_Down").GetComponent<Text>().text = tempS;
                    //make the second text visible
                    c = b.transform.Find("Text_Down").GetComponent<Text>().color;
                    c.a = 1;
                    b.transform.Find("Text_Down").GetComponent<Text>().color = c;
                }
            }

            data.LanguageForAchievement = "Français";
            dropDown.ClearOptions();
            //don't show image  ~= clear image
            c = imgDownLoad.color;
            c.a = 0;
            imgDownLoad.color = c;
        }
    }

}
