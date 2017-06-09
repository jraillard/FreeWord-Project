using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCategoryManagement : MonoBehaviour
{
    //script which manage the category button (instantiation+interaction)

    /********************************* Variables *********************************/
    private List<Button> catbuttonList = new List<Button>();
    private Transform tParent;
    private Button tempButton;
    private Sprite tempSprite;

    /********************************* Loop *********************************/

    private void Start()
    {
        float posX = 113;
        float posY = 358;
        int i = 0;

        tParent = GameObject.Find("Categories").GetComponent<Transform>();

        do
        {

            tempButton = Resources.Load("test/CategoryButton", typeof(Button)) as Button;

            catbuttonList.Add(Instantiate(tempButton, tParent));

            catbuttonList[i].GetComponent<RectTransform>().position = new Vector3(posX, posY, 0);

            //catbuttonList[i].GetComponent<Image>().sprite = tempSprite;
            catbuttonList[i].transform.Find("Text_Up").GetComponent<Text>().text = "Legumes";
            catbuttonList[i].transform.Find("Text_Down").GetComponent<Text>().text = "Vegatables";

            i++;

            if (i % 4 == 0 && i != 0)
            {
                posX = 113;
                posY -= 80;
            }
            else
            {
                posX += 110;
            }

        } while (i < 50);

    }

}
