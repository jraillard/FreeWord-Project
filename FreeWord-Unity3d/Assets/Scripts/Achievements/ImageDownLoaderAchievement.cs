using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageDownLoaderAchievement : MonoBehaviour
{
    /********************************* Variables *********************************/

    private Text downloadText;
    private Data data;
    private Dropdown dropDown;
    private string selectedS = "";
    private int selectedIdx;
    private string tempS = "";
    private List<Dropdown.OptionData> options;
    private List<string> url;
    private Dictionary<string, int> words;


    /********************************* Main Events *********************************/

    private void Start()
    {
        downloadText = GameObject.Find("Download").GetComponent<Text>();
        data = GameObject.Find("DataObject").GetComponent<Data>();
        dropDown = GameObject.Find("WordDropdown").GetComponent<Dropdown>();
    }

    private void Update()
    {
        //get the selected value
        options = dropDown.options;
        if(options.Count != 0)
        {
            selectedIdx = dropDown.value;
            selectedS = options[selectedIdx].text;

            if (selectedS != tempS && selectedS != "")
            {
                tempS = selectedS;
                words = data.GetWordListFromCategory();
                url = data.GetWordUrlListFromCategory();
                int i = 0;

                foreach (KeyValuePair<string, int> k in words)
                {
                    if (k.Key == selectedS)
                    {
                        break;
                    }
                    i++;
                }

                StartCoroutine(LoadImage(selectedS, url[i], data.CurrentCatName));

            }
        }
        
    }

    /********************************* Methods *********************************/

    public IEnumerator LoadImage(string word, string url, string catName)
    {
        // Check the directory ID
        if (!Directory.Exists(Application.persistentDataPath + "/WordTexture/" + data.CurrentCatName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/WordTexture/" + data.CurrentCatName);
        }

        if (File.Exists(Application.persistentDataPath + "/WordTexture/" + data.CurrentCatName + "/" + word + ".jpg"))
        {
            if (data.LanguageToPlay == "Français") { downloadText.text = "Chargement de l'image depuis l'appareil"; }
            else if (data.LanguageToPlay == "English") { downloadText.text = "Loading image from the device"; }

            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + "/WordTexture/" + data.CurrentCatName + "/" + word + ".jpg");
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(byteArray);
            this.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            if (data.LanguageToPlay == "Français") { downloadText.text = "Chargement de l'image depuis internet"; }
            else if (data.LanguageToPlay == "English") { downloadText.text = "Loading image from the web"; }
            WWW www = new WWW(url);
            yield return www; //wait that the image is downloaded
            Texture2D texture = www.texture;
            this.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            byte[] bytes = texture.EncodeToJPG();
            File.WriteAllBytes(Application.persistentDataPath + "/WordTexture/" + data.CurrentCatName + "/" + word + ".jpg", bytes);
        }

        downloadText.text = "";
    }
}
