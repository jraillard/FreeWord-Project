using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private Data data;
    private WWWForm form;
    private WWW w;

    /********************************* Loop *********************************/

    private void Start()
    {

        data = GameObject.Find("DataObject").GetComponent<Data>();
        StartCoroutine(InitCategoriesButton());
        //InitCategoriesButton();
    }

    //Instantiate and set up all category button in scrollList
    private IEnumerator InitCategoriesButton()
    {
        Dictionary<string, string> tempCatList = new Dictionary<string, string>();
        Dictionary<string, string> catList = new Dictionary<string, string>(); //contain LngToLearn : LngToPlay
        List<string> pngUrlList = new List<string>(); //contain Url
        string[] tempString;
        float posX = Screen.width * 250 / 1280;
        float posY = Screen.height * 480 / 720;
        int i = 0;

        tParent = GameObject.Find("Categories").GetComponent<Transform>();

        //WB cat request 
        
        form = new WWWForm();
        //set opositelanguage to languageToLearn
        form.AddField("username", data.Username);

        if (data.LanguageToPlay == "English")
        {
            data.LanguageForAchievement = "Français";
        }
        else
        {
            data.LanguageForAchievement = "English";
        }

        //print(data.LanguageForAchievement);
        form.AddField("language", data.LanguageForAchievement);

        //print(data.Username + "|" + data.LanguageToPlay);

        w = new WWW(data.GetDbURL+"GetCategories", form);
        yield return w;
        //print(w.text);

        tempCatList = JsonConvert.DeserializeObject<Dictionary<string, string>>(w.text);

        //Split url and 2cat name

        foreach (KeyValuePair<string, string> k in tempCatList)
        {
            //print(k.Key + " | " + k.Value);
            tempString = k.Value.ToString().Split('|');
            catList.Add(k.Key, tempString[0]);
            pngUrlList.Add(tempString[1]);

        }


        //find the path in hierarchy to instantiate button      
        tParent = GameObject.Find("Categories").GetComponent<Transform>();

        foreach (KeyValuePair<string, string> de in catList)
        {
            Texture2D texture;
            //Load prefab
            tempButton = Resources.Load("CategoryButtonAchievement", typeof(Button)) as Button;

            //Load Sprite
            if (!Directory.Exists(Application.persistentDataPath + "/CategoryTexture"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/CategoryTexture");
            }

            if (File.Exists(Application.persistentDataPath + "/CategoryTexture/" + de.Key + ".jpg"))
            {
                //print("Loading from the device");
                byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + "/CategoryTexture/" + de.Key + ".jpg");
                texture = new Texture2D(1, 1);
                texture.LoadImage(byteArray);
            }
            else
            {
                //print("Downloading from the web");
                WWW www = new WWW(pngUrlList[i]);
                yield return www; //wait that the image is downloaded
                texture = www.texture;
                byte[] bytes = texture.EncodeToJPG();
                File.WriteAllBytes(Application.persistentDataPath + "/CategoryTexture/" + de.Key + ".jpg", bytes);
            }

            //Instantiate
            catbuttonList.Add(Instantiate(tempButton, tParent));

            //Set button parameters
            catbuttonList[i].GetComponent<RectTransform>().position = new Vector3(posX, posY, 0);
            catbuttonList[i].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            catbuttonList[i].transform.Find("Text_Up").GetComponent<Text>().text = de.Key.ToString().Split('_')[0];
            if (data.LanguageToPlay != data.LanguageToLearn)
            {
                //if the same => just display one
                catbuttonList[i].transform.Find("Text_Down").GetComponent<Text>().text = de.Value.ToString().Split('_')[0];
            }


            //SetPosition
            i++;
            if (i % 3 == 0 && i != 0)
            {
                posX = Screen.width * 250 / 1280;
                posY -= Screen.height * 140 / 720;
            }
            else
            {
                posX += Screen.width * 180 / 1280;
            }
        }

     }

    public List<Button> GetCatButtonList()
    {
        return catbuttonList;
    }

}
