using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Tempo : MonoBehaviour
{

    private string[] Lines;
    private string[] Line;
    private string Username;

    void Start()
    {
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(3);
        if (System.IO.File.Exists(Application.persistentDataPath + "/ID/RM.txt")){
            Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/RM.txt");
            Username = Lines[0];
            System.IO.File.WriteAllText(Application.persistentDataPath + "/ID/JA.txt", Username);
            Line = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/" + Username + ".txt");
            int lng;
            lng = Line.Length;
            if (Line[lng - 1] == "English" || Line[lng - 1] == "Français"){
                GameObject.Find("Main Camera").GetComponent<GoToHomepage>().Load();
            }
            else { GameObject.Find("Main Camera").GetComponent<GoToLanguageToPlay>().Load(); }
        }
        else { GameObject.Find("Main Camera").GetComponent<GoToLogIn>().Load(); }
    }
}
