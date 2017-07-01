using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

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

    // Update is called once per frame
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

    public void LoginButton()
    {
        bool UN = false;
        bool PW = false;

        // Check the Username
        if (Username != "")
        {
            if (System.IO.File.Exists(Application.persistentDataPath+"/ID/" + Username + ".txt")) {
                UN = true;
                Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/" + Username + ".txt");
                EncryptedPass = Lines[1];
            }
            else { informations.GetComponent<Text>().text="Username Invalid"; }
        }
        else { informations.GetComponent<Text>().text="Username field empty"; }


        // Check the Password
        if (Password != "")
        {
            if (System.IO.File.Exists(Application.persistentDataPath + "/ID/" + Username + ".txt")) {
                int i = 1;
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
        if(UN==true && PW == true){
            int lng;
            lng = Lines.Length;
            theSM = FindObjectOfType<SoundManager>();
            if (GameObject.Find("Toggle_Remember").GetComponent<Toggle>().isOn){
                remember = (Username + Environment.NewLine + EncryptedPass);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/ID/RM.txt", remember);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/ID/JA.txt", Username);
                username.GetComponent<InputField>().text = "";
                password.GetComponent<InputField>().text = "";
                if (Smute == false)
                {
                    theSM.PlaySound();
                }
                informations.GetComponent<Text>().text = "Login Sucessfull";
                if(Lines[lng-1]=="English" || Lines[lng - 1] == "Français")
                {
                    GameObject.Find("Main Camera").GetComponent<GoToHomepage>().Load();
                }
                else { GameObject.Find("Main Camera").GetComponent<GoToLanguageToPlay>().Load(); }
            }
            else {
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
    }

    public void SetMute(bool b)
    {
        Smute = b;
    }

    public void FillLoginAfterRegister(string usr)
    {
        username.GetComponent<InputField>().text = usr;
    }
}