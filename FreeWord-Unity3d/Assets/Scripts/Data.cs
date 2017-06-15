using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    //Script which will manage all links between database and game through all the app

    /********************************* Variables *********************************/

    private List<string> wordLevelList; //list containing the words which could be played in the Game

    /********************************* Loops *********************************/
        
    //make the Data Object indestructible
    private void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	}

    /********************************* Methods *********************************/

    //receive the words from the category 
    public void GetWordListFromCategory(string catName, List<string> wordList, List<int> nbTimeWordDiscover)
    {
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
        else
        {
            wordList.Add("No Words in this category");
            nbTimeWordDiscover.Add(0);
        }

        wordLevelList = wordList;

    }

    //receive the words usable for the level
    public List<string> GetWordListFromGame()
    {
        return wordLevelList;
    }

}
