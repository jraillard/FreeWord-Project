using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    //Script which will manage all links between database and game through all the app

    /********************************* Variables *********************************/

    private List<string> wordLevelList = new List<string>(); //list containing the words which could be played in the Game
    public List<string> wordsDiscoveredDuringGame = new List<string>(); //list containing the words discovered during the current Game
    private string mysteryWord = "";
    private bool playAgainInSameCat = false;
    private string currentCatName = "";

    /******************************* Main Events *********************************/
        
    //make the Data Object indestructible
    private void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	}

    /********************************* Methods ***********************************/

    //receive the words from the category 
    public void GetWordListFromCategory(string catName, List<string> wordList, List<int> nbTimeWordDiscover)
    {
        /* must split between 2 list 
         * one with all the words 
         * one with their occurences
         * then split those in two more : 
         * one with the word to discover 
         * on with the word discovered
         */
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

        wordLevelList = wordList;
        currentCatName = catName;
    }

    //idem but without wordList and nbTimediscover list
    public void GetWordListFromCategory(string catName)
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
            wordList.Add("salut");
            nbTimeWordDiscover.Add(1);
            wordList.Add("coucou");
            nbTimeWordDiscover.Add(3);
            wordList.Add("maison");
            nbTimeWordDiscover.Add(4);
            wordList.Add("jardin");
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

        wordLevelList = wordList;
        currentCatName = catName;
    }
    //receive the words usable for the level
    public List<string> GetWordListFromGame()
    {
        if(!playAgainInSameCat)
        {
            return wordLevelList;
        }
        else
        {
            //need to reload the list with new nbtimediscover
            GetWordListFromCategory(currentCatName);
            return wordLevelList;
        }
        
    }

    //get/set the mystery word
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
            }
            else
            {
                print("isn't string");
            }
        }
    }

    //Add a word that have been discovered during the game
    public void AddWordDiscover(string wd)
    {
        wordsDiscoveredDuringGame.Add(wd);
    }

    //change the nbTimeDiscover attribute of each words
    public void FinishLevel()
    {
        playAgainInSameCat = true;
        //intial value after each games
        wordLevelList = new List<string>();
        wordsDiscoveredDuringGame = new List<string>();
        mysteryWord = "";
    }

    //when already done a game and then change category
    public void ChangeCategory()
    {
        playAgainInSameCat = false;
    }


}
