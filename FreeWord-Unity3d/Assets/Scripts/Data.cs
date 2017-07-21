using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


public class Data : MonoBehaviour {

    //Script which will manage all links between database and game through all the app

    /********************************* Variables *********************************/

    private Dictionary<string, int> wordLevelList; //list containing the words which could be played in the Game
    private Dictionary<string, int> wordPlayedList = new Dictionary<string, int>(); //list containing the words played during the current game
    private List<string> pngWordUrlList;
    private string mysteryWord = "";
    private string mysteryWordUrl = "";
    private bool playAgainInSameCat = false;
    private string currentCatName = "";
    private string currentLngToPlay = "";
    private string currentUsername = "";
    private string currentLngToLearn = "";
    private string currentLngAchievement = "";
    private int currentLevel;
    private bool verifExit = false;

    private bool detectWeb = true; //detect 
    private bool loadinternet = true;
    private bool verifload = false;

    private WWWForm form;
    private WWW w;
    private string dbURL = "http://localhost:60240/WoGamUser/";


    /******************************* Main Events *********************************/

    //make the Data Object indestructible
    private void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	}

    private void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            if (verifExit == false)
            {
                verifExit = true;
                Time.timeScale = 0f;
                SceneManager.LoadSceneAsync("Exit", LoadSceneMode.Additive);
            }
            else
            {
                verifExit = false;
                Time.timeScale = 1.0f;
                SceneManager.UnloadSceneAsync("Exit");
            }
        }

        //Check internet connection during the game
        if (Application.internetReachability == NetworkReachability.NotReachable && detectWeb == true)
        {
            detectWeb = false;
        }
        if (verifload == false && detectWeb == false)
        {
            verifload = true;
            loadinternet = detectWeb;
        }
        if (loadinternet == false)
        {
            loadinternet = true;
            SceneManager.LoadSceneAsync("Internet", LoadSceneMode.Additive);
        }
    }

    /********************************* Methods ***********************************/

    public string LanguageForAchievement
    {
        get { return currentLngAchievement; }
        set
        {
            if(value == "English" || value == "Français")
            {
                currentLngAchievement = value;
            }
        }
    }

    public void SetLanguageToPlayAndUsrName()
    {
            string[] Lines;
            string[] Line;

            Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/JA.txt");
            currentUsername= Lines[0];
            Line = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/" + Username + ".txt");
            int lng;
            lng = Line.Length;
            if (Line[lng - 1] == "English") { currentLngToPlay = "English"; }
            else if (Line[lng - 1] == "Français") { currentLngToPlay = "Français"; }        
    }

    public string LanguageToPlay
    {
        get { return currentLngToPlay; }
    }

    public string Username
    {
        get { return currentUsername; }
    }

    public string LanguageToLearn
    {
        get { return currentLngToLearn; }
        set { if (value is string) currentLngToLearn = value; }
    }

    public string CurrentCatName
    {
        get { return currentCatName; }
        set { if (value is string) currentCatName = value; }
    }

    public int CurrentLevel
    {
        get { return currentLevel;  }
        set { currentLevel = value; }
    }

    public string GetDbURL
    {
        get { return dbURL; }
    }

    //set the words from the category selected => necessary because of coroutine
    public void SetWordListFromCategory(Dictionary<string, int> dWord, List<string> lUrl)
    {
        wordLevelList = new Dictionary<string, int>();
        pngWordUrlList = new List<string>();

        foreach (KeyValuePair<string, int> k in dWord)
        {
            wordLevelList.Add(k.Key, k.Value);
        }

        foreach (string s in lUrl)
        {
            pngWordUrlList.Add(s);
        }

       // print(wordLevelList.Count.ToString() + " | " + pngWordUrlList.Count.ToString());
    }


    //idem but without wordList and nbTimediscover list
    public void GetWordListFromCategoryTest(string catName)
    {
        List<string> wordList = new List<string>();
        List<int> nbTimeWordDiscover = new List<int>();

        if (catName == "Category_0")
        {
            //must add word then his timeDiscovernb
            wordList.Add("excellent");
            nbTimeWordDiscover.Add(0);
            wordList.Add("bonjour");
            nbTimeWordDiscover.Add(2);
            wordList.Add("mot clé");
            nbTimeWordDiscover.Add(1);
            wordList.Add("tir-à-l'arc");
            nbTimeWordDiscover.Add(3);
            wordList.Add("l'oeil");
            nbTimeWordDiscover.Add(4);
            wordList.Add("aujourd'hui");
            nbTimeWordDiscover.Add(5);
        }
        else if (catName == "Category_1")
        {
            wordList.Add("null");
            nbTimeWordDiscover.Add(0);
            wordList.Add("tomate");
            nbTimeWordDiscover.Add(2);
            wordList.Add("concombre");
            nbTimeWordDiscover.Add(7);
            wordList.Add("courgette");
            nbTimeWordDiscover.Add(8);
            wordList.Add("poireau");
            nbTimeWordDiscover.Add(9);
            wordList.Add("carotte");
            nbTimeWordDiscover.Add(3);
        }
        else if (catName == "Category_2")
        {
            wordList.Add("hallucinant");
            nbTimeWordDiscover.Add(0);
            wordList.Add("basket");
            nbTimeWordDiscover.Add(3);
            wordList.Add("foot");
            nbTimeWordDiscover.Add(4);
            wordList.Add("handball");
            nbTimeWordDiscover.Add(5);
            wordList.Add("rugby");
            nbTimeWordDiscover.Add(7);
            wordList.Add("judo");
            nbTimeWordDiscover.Add(8);
        }
        else if (catName == "Category_3")
        {
            wordList.Add("mot mystère");
            nbTimeWordDiscover.Add(0);
            wordList.Add("test");
            nbTimeWordDiscover.Add(1);

        }
        else
        {
            wordList.Add("No Words in this category");
            nbTimeWordDiscover.Add(0);
        }

        //wordLevelList = wordList;
        currentCatName = catName;
    }

    //receive the words usable for the level
    public Dictionary<string, int> GetWordListFromCategory()
    {
        if (!playAgainInSameCat)
        {
            return wordLevelList;
        }
        else
        {
            //need to reload the list with new nbtimediscover
            //StartCoroutine(GetWordListFromCategory());
            return wordLevelList;
        }

    }

    //receive the words usable for the level
    public List<string> GetWordUrlListFromCategory()
    {
        if (!playAgainInSameCat)
        {
            return pngWordUrlList;
        }
        else
        {
            //need to reload the list with new nbtimediscover
            //StartCoroutine(GetWordListFromCategory());
            return pngWordUrlList;
        }

    }

    //set the word which are played during the game
    public void SetWordListPlayed(Dictionary<string, int> d)
    {
        foreach(KeyValuePair<string, int> k in d)
        {
            wordPlayedList.Add(k.Key, k.Value);
        }
    }

    public Dictionary<string, int> GetWordListPlayed()
    {
        return wordPlayedList;
    }

    //get/set the mystery word/url
    public string MysteryWord
    {
        get
        {
            return mysteryWord;
        }

        set
        {
            if(value is string)
            {
                mysteryWord = value;
                wordPlayedList.Add(value, 0); //add to playedwordlist
            }
            else
            {
                print("isn't string");
            }
        }
    }

    public string MysteryWordUrl
    {
        get
        {
            return mysteryWordUrl;
        }

        set
        {
            if (value is string)
            {
                mysteryWordUrl = value;
            }
            else
            {
                print("isn't string");
            }
        }
    }


    //change the nbTimeDiscover attribute of each words
    public void FinishLevel()
    {
        //if reach this function => all words have been discovered otherwise : it's like no words has been discovered
        playAgainInSameCat = true;
        //intial value after each games (even if stay in same cat =>gotoselection will reset it)
        wordLevelList = new Dictionary<string, int>();
        wordPlayedList = new Dictionary<string, int>();
        pngWordUrlList = new List<string>();
        currentLevel++;
        mysteryWord = "";
    }

    //when already done a game and then change category
    public void ChangeCategory()
    {
        playAgainInSameCat = false;
    }

    //verify if exit scene is already loaded
    public bool SetVerifExit
    {
        get { return verifExit; }

        set { verifExit = value; }
    }

    public bool SetdetectWeb
    {
        get { return detectWeb; }

        set { detectWeb = value; }
    }

    public bool Setverifload
    {
        get { return verifload; }

        set { verifload = value; }
    }
}
