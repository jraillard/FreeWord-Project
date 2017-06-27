using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Register : MonoBehaviour {

    public GameObject username;
    public GameObject password;
    public GameObject confPassword;
    public GameObject informations;

    private string Username;
    private string Password;
    private string ConfPassword;
    private string form;

	// Use this for initialization
	void Start () {
        // Check the directory ID
        if (Directory.Exists(Application.persistentDataPath + "/ID")) { return; }
        print(Application.persistentDataPath);

        // Try to create the directory.
        Directory.CreateDirectory(Application.persistentDataPath + "/ID");

    }

    public void RegisterButton()
    {
        bool UN = false;
        bool PW = false;
        bool CPW = false;
        
        // Check the Username
        if (Username != "") {
            if (!File.Exists(Application.persistentDataPath+ "/ID/"+Username+".txt")){
                UN = true;
            }
            else{ informations.GetComponent<Text>().text="Username already used";}
        }
        else{ informations.GetComponent<Text>().text = "Username field empty";}

        // Check the Password
        if (Password != ""){
            if(Password.Length > 5){
                PW = true;
            }
            else { informations.GetComponent<Text>().text="Password must be atleast 6 characters long"; }
        }
        else { informations.GetComponent<Text>().text="Password field is empty"; }

        // Check the Confirm Password
        if (ConfPassword != ""){
            if (ConfPassword == Password){
                CPW = true;
            }
            else { informations.GetComponent<Text>().text="Passwords don't match"; }
        }
        else { informations.GetComponent<Text>().text="Confirm Password field is empty"; }

        // Encrypting the password
        if (UN==true && PW==true && CPW == true){
            bool Clear = true;
            int i = 1;
            foreach(char c in Password){
                if (Clear) {
                    Password = "";
                    Clear = false;
                }
                i++;
                char Encrypted = (char)(c * i);
                Password += Encrypted.ToString();
            }
            form = (Username +Environment.NewLine + Password);
            File.WriteAllText(Application.persistentDataPath+"/ID/" + Username + ".txt", form);
            username.GetComponent<InputField>().text="";
            password.GetComponent<InputField>().text="";
            confPassword.GetComponent<InputField>().text="";
            informations.GetComponent<Text>().text = "Registration Complete";
            //fill the login field
            GameObject.Find("Login").GetComponent<Login>().FillLoginAfterRegister(Username);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Username = username.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfPassword = confPassword.GetComponent<InputField>().text;
        // Go to the next Inputfield when press Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.GetComponent<InputField>().isFocused) { password.GetComponent<InputField>().Select(); }
            if (password.GetComponent<InputField>().isFocused) { confPassword.GetComponent<InputField>().Select(); }
        }
        // Press RegisterButton when all the Inputfield are completed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Username != "" && Password !="" && ConfPassword !="") { RegisterButton(); }

        }
    }
}
