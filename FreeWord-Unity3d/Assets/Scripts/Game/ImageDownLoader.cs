using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ImageDownLoader : MonoBehaviour {
    //script to download / load the image we have to discover in placedCard

    private Text downloadText;
    private Data data;

    private void Start()
    {
        downloadText = GameObject.Find("Download").GetComponent<Text>();
        data = GameObject.Find("DataObject").GetComponent<Data>();
    }
    public IEnumerator LoadImage(string word, string url, string catName)
    { 
        // Check the directory ID
        if (!Directory.Exists(Application.persistentDataPath + "/WordTexture/" + catName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/WordTexture/" + catName);
        }        

        if (File.Exists(Application.persistentDataPath + "/WordTexture/" + catName + "/" + word + ".jpg"))
        {
            if(data.LanguageToPlay == "Français") { downloadText.text = "Chargement de l'image depuis l'appareil"; }
            else if (data.LanguageToPlay == "English") { downloadText.text = "Loading image from the device"; }

            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + "/WordTexture/" + catName + "/" + word + ".jpg");
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
            File.WriteAllBytes(Application.persistentDataPath + "/WordTexture/" + catName + "/" + word + ".jpg", bytes);
        }

        downloadText.text = "";
    }

}
