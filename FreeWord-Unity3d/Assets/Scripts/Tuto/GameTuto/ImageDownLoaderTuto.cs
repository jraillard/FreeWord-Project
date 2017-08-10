using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ImageDownLoaderTuto : MonoBehaviour
{
    //script to download / load the image we have to discover in placedCard

    /********************************* Variables *********************************/

    private Data data;
    private Text downloadText;

    /********************************* Methods *********************************/

    public void SetImage(string word)
    {
        switch (word)
        {
            case "orange": StartCoroutine(LoadingImage("http://www.jgfruitsetlegumes.com/wp-content/uploads/2016/05/orange-300x300.jpg", "orange"));
                break;

            case "restaurant": StartCoroutine(LoadingImage("https://www.darwinescapes.co.uk/wp-content/uploads/Signatures-Restaurant.jpg", "restaurant"));
                break;
                 
            case "rat": StartCoroutine(LoadingImage("http://media.petsathome.com/wcsstore/pah-cas01//300/1106388PL.jpg", "rat"));
                break;
        }
    }

    private IEnumerator LoadingImage(string url, string textureName)
    {
        downloadText = GameObject.Find("Download").GetComponent<Text>();
        data = GameObject.Find("DataObject").GetComponent<Data>();

        if (!Directory.Exists(Application.persistentDataPath + "/Tuto_Texture"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Tuto_Texture");
        }

        if (File.Exists(Application.persistentDataPath + "/Tuto_Texture/" + textureName + ".jpg"))
        {
            //downloadText.text = "coucou";
            if (data.LanguageToPlay == "Français") { downloadText.text = "Chargement de l'image depuis l'appareil"; }
            else if (data.LanguageToPlay == "English") { downloadText.text = "Loading image from the device"; }
            //print("Loading from the device");
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + "/Tuto_Texture/" + textureName + ".jpg");            
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(byteArray);
            this.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        }
        else
        {
            //downloadText.text = "coucou2";
            //print(data.LanguageToPlay);
            if (data.LanguageToPlay == "Français") { downloadText.text = "Chargement de l'image depuis internet"; }
            else if (data.LanguageToPlay == "English") { downloadText.text = "Loading image from the web"; }

            //print("Downloading from the web");
            WWW www = new WWW(url);
            yield return www; //wait that the image is downloaded
            Texture2D texture = www.texture;
            this.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            byte[] bytes = texture.EncodeToJPG();
            File.WriteAllBytes(Application.persistentDataPath + "/Tuto_Texture/" + textureName + ".jpg", bytes);
        }

        downloadText.text = "";
    }
}
