using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LtP_Choice : MonoBehaviour {

    public GameObject choice;

    private string Choice;
    private string Username;
    private string EncryptedPassword;
    private string[] Lines;
    private string[] Line;


    public void English()
    {
        Lines=System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/JA.txt");
        Username = Lines[0];
        Line = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/" + Username + ".txt");
        EncryptedPassword = Line[1];
        Choice = (Username + Environment.NewLine + EncryptedPassword + Environment.NewLine + "English");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/ID/" + Username + ".txt", Choice);
        StartCoroutine(SetLanguageToDB("English"));   
    }

    public void Français()
    {
        Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/JA.txt");
        Username = Lines[0];
        Line = System.IO.File.ReadAllLines(@Application.persistentDataPath + "/ID/" + Username + ".txt");
        EncryptedPassword = Line[1];
        Choice = (Username + Environment.NewLine + EncryptedPassword + Environment.NewLine + "Français");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/ID/" + Username + ".txt", Choice);
        StartCoroutine(SetLanguageToDB("Français"));
    }

    public IEnumerator SetLanguageToDB(string lng)
    {
        WWWForm webForm;
        WWW w;

        webForm = new WWWForm();
        webForm.AddField("username", Username);
        webForm.AddField("Language", lng);
        w = new WWW("http://localhost:60240/WoGamUser/SetGameLanguage", webForm);
        yield return w;
        //print(w.text);
    }
}
