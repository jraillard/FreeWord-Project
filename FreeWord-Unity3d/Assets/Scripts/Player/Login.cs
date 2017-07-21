using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class Login : MonoBehaviour
{

    public GameObject username;
    public GameObject password;
    public GameObject informations;
    public bool Smute;

    private SoundManager theSM;
    private string Username;
    private string Password;
    private string[] Lines;
    private string DecryptedPass;
    private string remember;
    private string EncryptedPass;

    private int lng;
    //private string gameLanguage = "";

    private bool UN = false;
    private bool PW = false;

    private WWWForm webForm;
    private WWW w;
    private WWW w2;
    private Data data;

    // Update is called once per frame
    private void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.GetComponent<InputField>().isFocused) { password.GetComponent<InputField>().Select(); }
        }
        // Press RegisterButton when all the Inputfield are completed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Username != "" && Password != "") { LoginButton(); }

        }
        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
    }

    public void LoginFunction()
    {
        StartCoroutine(LoginButton());
    }

    public IEnumerator LoginButton()
    {
        // Check the Username
        if (Username != "")
        {
            if (System.IO.File.Exists(Application.persistentDataPath+"/ID/" + Username + ".txt")) {
                UN = true;
                Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/" + Username + ".txt");
                EncryptedPass = Lines[1];
                UN = true;
            }
            else
            {
                if(Password!="")
                {
                    //trytologin in db                   

                    webForm = new WWWForm();
                    webForm.AddField("username", Username);
                    webForm.AddField("password", Password);
                    w = new WWW(data.GetDbURL+"LoginUser", webForm);
                    yield return w;
                    //print(w.text);

                    webForm = new WWWForm();
                    webForm.AddField("username", Username);
                    w2 = new WWW(data.GetDbURL+"GetGameLanguage", webForm);
                    yield return w2;
                    print(w2.text);

                    if (w.text == "Done")
                    {
                        CreateIdFile();
                    }
                    else
                    {
                        informations.GetComponent<Text>().text = "Username or Password Invalid";
                    }
                }
                else
                {
                    informations.GetComponent<Text>().text = "Password is empty";
                }
            }
                
        }
        else { informations.GetComponent<Text>().text="Username field empty"; }


        // Check the Password
        if (Password != "")
        {
            if (System.IO.File.Exists(Application.persistentDataPath + "/ID/" + Username + ".txt")) {
                int i = 1;
                Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/" + Username + ".txt");
                foreach (char c in Lines[1]){
                    i++;
                    char Decrypted = (char)(c / i);
                    DecryptedPass += Decrypted.ToString();
                }
                if (Password == DecryptedPass){
                    PW = true;
                }
                else { informations.GetComponent<Text>().text="Password is invalid"; }
            }
            else { informations.GetComponent<Text>().text="Password is invalid"; }
        }
        else { informations.GetComponent<Text>().text="Password filed is empty"; }

        if (UN == true && PW == true) {
            Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/" + Username + ".txt");
            lng = Lines.Length;
            theSM = FindObjectOfType<SoundManager>();

            if (GameObject.Find("Toggle_Remember").GetComponent<Toggle>().isOn) {
                remember = (Username + Environment.NewLine + EncryptedPass);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/ID/RM.txt", remember);
         
            }
            System.IO.File.WriteAllText(Application.persistentDataPath + "/ID/JA.txt", Username);
            username.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            if (Smute == false)
            {
                theSM.PlaySound();
            }
            informations.GetComponent<Text>().text = "Login Sucessfull";
            if (Lines[lng - 1] == "English" || Lines[lng - 1] == "Français")
            {
                GameObject.Find("Main Camera").GetComponent<GoToHomepage>().Load();
            }
            else { GameObject.Find("Main Camera").GetComponent<GoToLanguageToPlay>().Load(); }
        }
       
      }       
    

    public void SetMute(bool b)
    {
        Smute = b;
    }

    public void FillLoginAfterRegister(string usr)
    {
        username.GetComponent<InputField>().text = usr;
    }

/*
    public IEnumerator GetLanguageOnDB()
    {
        WWWForm webForm;
        WWW w;

        webForm = new WWWForm();
        webForm.AddField("username", Username);
        w = new WWW("http://localhost:60240/WoGamUser/GetGameLanguage", webForm);
        yield return w;
        
        if(w.text != "error") { gameLanguage = w.text; }
    }
*/

    public void CreateIdFile()
    {
        // Encrypting the password and create file related to user
        string form = "";
        bool Clear = true;
        int i = 1;
        foreach (char c in Password)
        {
            if (Clear)
            {
                Password = "";
                Clear = false;
            }
            i++;
            char Encrypted = (char)(c * i);
            Password += Encrypted.ToString();
        }
        if(w2.text != "Error") { form=(Username + Environment.NewLine + Password + Environment.NewLine + w2.text); }
        else { form = (Username + Environment.NewLine + Password); }
        
        File.WriteAllText(Application.persistentDataPath + "/ID/" + Username + ".txt", form);
        UN = true;
        PW = true;
    }
    
}