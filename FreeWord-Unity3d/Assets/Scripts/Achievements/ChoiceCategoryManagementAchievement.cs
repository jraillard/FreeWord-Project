using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCategoryManagementAchievement : MonoBehaviour
{
    //script which manage the category button (instantiation+interaction)

    /********************************* Variables *********************************/
    private List<Button> catbuttonList = new List<Button>(); //list containing all categorybutton
    private Transform tParent; //variables needed for loading prefab/sprites
    private Button tempButton;
    private Sprite tempSprite;

    /********************************* Loop *********************************/

    private void Start()
    {
        InitCategoriesButton();

    }

    //Instantiate and set up all category button in scrollList
    private void InitCategoriesButton()
    {
        float posX = 207f;
        float posY = 495f;
        int i = 0;

        tParent = GameObject.Find("Categories").GetComponent<Transform>();

        do
        {

            tempButton = Resources.Load("CategoryButtonAchievement", typeof(Button)) as Button;

            catbuttonList.Add(Instantiate(tempButton, tParent));

            catbuttonList[i].GetComponent<RectTransform>().position = new Vector3(posX, posY, 0);

            //catbuttonList[i].GetComponent<Image>().sprite = tempSprite;
            catbuttonList[i].transform.Find("Text_Up").GetComponent<Text>().text = "Category_" + i;
            catbuttonList[i].transform.Find("Text_Down").GetComponent<Text>().text = "Categorie_" + i;

            i++;

            if (i % 4 == 0 && i != 0)
            {
                posX = 207f;
                posY -= 110;
            }
            else
            {
                posX += 165;
            }

        } while (i < 50);
    }

}
