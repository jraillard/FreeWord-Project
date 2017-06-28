using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class ImageDownLoaderTuto : MonoBehaviour
{
    //script to download / load the image we have to discover in placedCard
    //private GameObject Debug;

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
        //Debug = GameObject.Find("Debug");
        //Debug.GetComponent<Text>().text = "Start loading function";
        //yield return new WaitForSeconds(3);
        //check if directory exists
        if (!Directory.Exists(Application.persistentDataPath + "/Tuto_Texture"))
        {
            //Debug.GetComponent<Text>().text = "Folder don't exist";
            //yield return new WaitForSeconds(3);
            Directory.CreateDirectory(Application.persistentDataPath + "/Tuto_Texture");
        }

        //File.Delete(Application.persistentDataPath + "/Tuto_Texture/" + textureName + ".jpg");
        if (File.Exists(Application.persistentDataPath + "/Tuto_Texture/" + textureName+".jpg"))
        {
            print("Loading from the device");
            //Debug.GetComponent<Text>().text = "Loading from the device";
            //yield return new WaitForSeconds(3);
            //Debug.GetComponent<Text>().text = Application.persistentDataPath + "/Tuto_Texture/" + textureName + ".jpg";
            //yield return new WaitForSeconds(3);
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + "/Tuto_Texture/" + textureName + ".jpg");            
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(byteArray);
            this.GetComponent<Renderer>().material.mainTexture = texture;
        }
        else
        {
            print("Downloading from the web");
            //Debug.GetComponent<Text>().text = "Loading from the web";
            //yield return new WaitForSeconds(3);
            WWW www = new WWW(url);
            yield return www; //wait that the image is downloaded
            Texture2D texture = www.texture;
            this.GetComponent<Renderer>().material.mainTexture = texture;
            byte[] bytes = texture.EncodeToJPG();
            File.WriteAllBytes(Application.persistentDataPath + "/Tuto_Texture/" + textureName + ".jpg", bytes);
        }

        //Debug.GetComponent<Text>().text = "End Loading";
        //yield return new WaitForSeconds(3);
    }
}
