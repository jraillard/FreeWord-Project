using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Register : MonoBehaviour {

    /********************************* Variables *********************************/

    public GameObject username;
    public GameObject password;
    public GameObject confPassword;
    public GameObject informations;

    private string Username;
    private string Password;
    private string ConfPassword;
    private string form;
    private bool US = false; //user used in DB
    private Data data;

    private WWWForm webForm;
    private WWW w;

    /********************************* Main Events *********************************/

    // Use this for initialization
    void Start () {

        data = GameObject.Find("DataObject").GetComponent<Data>();
        // Check the directory ID
        if (Directory.Exists(Application.persistentDataPath + "/ID")) { return; }
        print(Application.persistentDataPath);

        // Try to create the directory.
        Directory.CreateDirectory(Application.persistentDataPath + "/ID");

    }

    // Update is called once per frame
    void Update()
    {
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
            if (Username != "" && Password != "" && ConfPassword != "") { RegisterButton(); }

        }
    }

    /********************************* Methods *********************************/

    public void RegisterAccount()
    {
        StartCoroutine(RegisterButton());
    }

    public IEnumerator RegisterButton()
    {
        
        bool UN = false;
        bool PW = false;
        bool CPW = false;
        string pattern = @"^[a-zA-Z0-9]{1,20}$";
        
        
        // Check the Username
        if (Username != "" )
        {
            Match m = Regex.Match(Username, pattern);
            if (m.Success)
            {
                
                if (!File.Exists(Application.persistentDataPath + "/ID/" + Username + ".txt"))
                {
                    UN = true;
                }
                else { informations.GetComponent<Text>().text = "Username already used1"; }

            }else 
            {
                informations.GetComponent<Text>().text = "Username must be alphanumeric and less than 20 characters";
            }
            
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

        //try to create user on DB
        webForm = new WWWForm();
        webForm.AddField("username", Username);
        webForm.AddField("password", Password);
        w = new WWW(data.GetDbURL + "CreateUser", webForm);
        yield return w;

        if (w.text == "Done") { US = true; }
        if (US==false) { informations.GetComponent<Text>().text = "Username already used2"; }

        // Encrypting the password
        if (UN==true && PW==true && CPW == true && US == true){
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
            GameObject.Find("Register_button").GetComponent<AudioSource>().Play();
            username.GetComponent<InputField>().text="";
            password.GetComponent<InputField>().text="";
            confPassword.GetComponent<InputField>().text="";
            informations.GetComponent<Text>().text = "Registration Complete";
            //fill the login field
            GameObject.Find("Login").GetComponent<Login>().FillLoginAfterRegister(Username);

        }
    }

    public IEnumerator RegisterOnDB()
    {
        WWWForm webForm;
        WWW w;

        webForm = new WWWForm();
        webForm.AddField("username", Username);
        webForm.AddField("password", Password);
        w = new WWW(data.GetDbURL+"CreateUser", webForm);
        yield return w;

        if(w.text == "Done") { US = true; }
    }
}
