using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageDownLoader : MonoBehaviour {
//script to download / load the image we have to discover in placedCard

    IEnumerator Start()
    {
        if(File.Exists(Application.persistentDataPath + "testTexture.jpg"))
        {
            print("Loading from the device");
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + "testTexture.jpg");
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(byteArray);
            this.GetComponent<Renderer>().material.mainTexture = texture;
        }
        else
        {
            print("Downloading from the web");
            WWW www = new WWW("https://www.dreamhost.com/blog/wp-content/uploads/2015/10/DHC_blog-image-01-300x300.jpg");
            yield return www; //wait that the image is downloaded
            Texture2D texture = www.texture;
            this.GetComponent<Renderer>().material.mainTexture = texture;
            byte[] bytes = texture.EncodeToJPG();
            File.WriteAllBytes(Application.persistentDataPath + "testTexture.jpg", bytes);
        }
    }
}
