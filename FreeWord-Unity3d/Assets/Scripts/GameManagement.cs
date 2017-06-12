using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    /********************************* Variables *********************************/

    //the 3 set of card we needs
    //they must be GameObject to be used after
    private List<GameObject> mysteryCardSet = new List<GameObject>();
    private List<GameObject> playedCardSet = new List<GameObject>();
    private List<GameObject> placedCardSet = new List<GameObject>();

    //need (almost) to store Prefab before Instantiate 
    private GameObject tempObj;
    private Sprite tempSprite;

    private int cpt; //counter for update loop
    private List<string> listWord;
    private List<char> splitWord;

    /********************************* Loops *********************************/

    void Start()
    {
        /*
        splitWord = MySplitter("bravo");
        InitMysteryCardSet(splitWord);
        InitPlayedCardSet(splitWord);
        InitPlacedCardSet(splitWord);
        InitWordBonusButton();
        */

        //Test1
        listWord = new List<string>();
        listWord.Add("excellent");
        listWord.Add("bonjour");
        listWord.Add("respect");
        listWord.Add("légende");
        listWord.Add("maison");
        listWord.Add("jardin");
        listWord.Add("espagne");
        listWord.Add("soirée");
        listWord.Add("tomate");
        listWord.Add("finlande");
        listWord.Add("barbecue");


        //Initiate the game 


        //Instantiate all the Card Sets

        //first of list = mysterword
        splitWord = MySplitter(listWord[0]);
        listWord.RemoveAt(0);

        
        InitMysteryCardSet(splitWord);
        float incY = 0, incZ = 0;      
               
        foreach (string s in listWord)
        {
            //print(s);
            splitWord = MySplitter(s);
            InitPlayedCardSet(splitWord, incY, incZ);
            incY -= 0.08f;
            incZ -= 0.1f;
        }
        
        splitWord = MySplitter(listWord[listWord.Count - 1]);
        InitPlacedCardSet(splitWord);
        InitWordBonusButton();
        

    }

    
    // Update is called once per frame    
    void Update()
    {
        //verify if every PlacedCard have a PlayedCard on it 
        foreach (GameObject obj in placedCardSet)
        {
            if (obj.GetComponent<PlacingCard>().IsPlaceAvailable() == false)
            {
                cpt++;
            }
        }
        //print(cpt);

        if (cpt == placedCardSet.Count) //if it is 
        {
            if (VerifPlacedWord() == true)
            {
                RemovePlacedWord();
                if (playedCardSet.Count == 0) //if there's no more PlayedCard 
                {
                    DiscoverMysteryWord();
                }
                else
                {
                    listWord.RemoveAt(listWord.Count - 1);
                    splitWord = MySplitter(listWord[listWord.Count - 1]);
                    InitPlacedCardSet(splitWord);
                    InitWordBonusButton();
                }
            }
        }

        cpt = 0;
    }    

    /********************************* Methods *********************************/

    //Split a word into a list of char 
    private List<char> MySplitter(string word)
    {
        List<char> splitWord = new List<char>();
        for (int j = 0; j < word.Length; j++)
        {
            splitWord.Add(word[j]);
        }

        return splitWord;
    }

    //function to receive the CardSet by giving the right cardSetName
    public List<GameObject> GetCardSet(string cardSetName)
    {
        //"break;" not needed because of the "return"
        switch (cardSetName)
        {
            case "played":
                return playedCardSet;

            case "placed":
                return placedCardSet;


            case "mystery":
                return mysteryCardSet;


            default:
                print("not a card set");
                return null;
        }
    }

    //Instantiate all the MysteryCard to discover
    private void InitMysteryCardSet(List<char> letterList)
    {
        float posX = 0.8f, posY = 0.75f, posZ = 3f;
        int i = 0;

        foreach (char c in letterList)
        {
            //Load the right prefab + sprite  and Instantiate 

            tempObj = Resources.Load("test/PlayedCard") as GameObject;

            if (c == 'ÿ') //particular case
            {
                tempSprite = Resources.Load("Letter/Letter_y¨", typeof(Sprite)) as Sprite;
            }
            else 
            {
                tempSprite = Resources.Load("Letter/Letter_" + c , typeof(Sprite)) as Sprite;
            }

            //print(c);
            //print(tempSprite);
            
            mysteryCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the sprite
            mysteryCardSet[i].GetComponent<SpriteRenderer>().sprite = tempSprite;

            //Set the value of the Card 
            mysteryCardSet[i].GetComponent<PlayedCard>().SetValue(c);
            //print(mysteryCardSet[i].GetComponent<PlayedCard>().GetValue());

            //Make the PlacedCard hidden
            mysteryCardSet[i].GetComponent<PlayedCard>().SetSelectable(false);
            mysteryCardSet[i].GetComponent<PlayedCard>().SetVisibility(false);
            //print(mysteryCardSet[i].GetComponent<PlayedCard>().IsVisible()+"1");
            //mysteryCardSet[i].GetComponent<SpriteRenderer>().color = Color.black;

            i++;
            posZ +=0.1f;
        }

    }

    //Instantiate all the PlayedCard to discover
    private void InitPlayedCardSet(List<char> letterList, float newposY, float newposZ)
    {
        float posX = 0.075f, posY = 0.35f+newposY, posZ = 5f+newposZ;
        int i = playedCardSet.Count;

        foreach (char c in letterList)
        {

            //Load the right prefab + sprite  and Instantiate 
            if (c == 'ÿ') //particular case
            {
                tempSprite = Resources.Load("Letter/Letter_y¨", typeof(Sprite)) as Sprite;
            }
            else
            {
                tempSprite = Resources.Load("Letter/Letter_" + c, typeof(Sprite)) as Sprite;
            }

            //print(c);
            //print(tempSprite);

            tempObj = Resources.Load("test/PlayedCard") as GameObject;

            playedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the sprite
            

            playedCardSet[i].GetComponent<SpriteRenderer>().sprite = tempSprite;

            //Set the value of the Card 
            playedCardSet[i].GetComponent<PlayedCard>().SetValue(c);
            //print(playedCardSet[i].GetComponent<PlayedCard>().GetValue());
            //print(playedCardSet[i].GetComponent<PlayedCard>().IsVisible() + "2");

            //Make able to DragAndDrop the card
            playedCardSet[i].GetComponent<PlayedCard>().SetSelectable(true);
            playedCardSet[i].GetComponent<PlayedCard>().SetVisibility(true);

            i++;
            posX += 0.211f;

        }

    }

    //Instantiate all the PlacedCard to discover
    private void InitPlacedCardSet(List<char> letterList)
    {
        float posX = 1.55f, posY = -0.8f, posZ = 3f;
        int i = 0;

        for(int j=letterList.Count-1; j>=0; j--)
        {
            //Load the right prefab + sprite  and Instantiate 

            tempObj = Resources.Load("test/PlacedCard") as GameObject;


            tempSprite = Resources.Load("Shape/Shape", typeof(Sprite)) as Sprite;

            //print(c);
            //print(tempSprite);

            placedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the sprite
            placedCardSet[i].GetComponentInChildren<SpriteRenderer>().sprite = tempSprite;

            //Set the value of the Card 
            placedCardSet[i].GetComponentInChildren<PlacedCard>().SetValue(letterList[j]);
            //print(placedCardSet[i].GetComponentInChildren<PlacedCard>().GetValue());

            //placedCardSet[i].GetComponentInChildren<SpriteRenderer>().color = Color.white;

            //Make the card Hidden (shown only with WordBonus)
            placedCardSet[i].GetComponentInChildren<PlacedCard>().SetVisibility(false);

            i++;
            posX -= 0.211f;
        }
    }

    //Initiate the WordBonus availability (don't need for ReplayBonus)
    private void InitWordBonusButton()
    {
        GameObject.Find("MainCamera").GetComponent<WordBonus>().SetBonusAvailability(true);
    }

    //Verify if the PlacedWord built with the PlayedCard is right or no
    private bool VerifPlacedWord()
    {
        int cptVerif = 0;

        foreach (GameObject obj in placedCardSet)
        {
            if (obj.GetComponentInChildren<PlacedCard>().IsWellPlaced() == true)
            {
                cptVerif++;
            }
        }

        if (cptVerif == placedCardSet.Count)
        {
            print("Great!");
            return true;
        }
        else
        {
            print("Not a word!");
            return false;
        }

    }

    //Delete the PlacedCard and the PlayedCard on it
    private void RemovePlacedWord()
    {
        List<GameObject> toRemove = new List<GameObject>();

        //Remove PlacedCard
        foreach (GameObject obj in placedCardSet)
        {
            toRemove.Add(obj);
            Destroy(obj);
        }

        foreach (GameObject obj in toRemove)
        {
            placedCardSet.Remove(obj);
        }


        toRemove = new List<GameObject>(); //DON'T FORGET !!!
        //Remove PlayedCard wellPlaced 

        foreach (GameObject obj in playedCardSet)
        {
            if (obj.GetComponent<PlayedCard>().IsPlaced() == true)
            {
                toRemove.Add(obj);
                Destroy(obj);
            }

        }

        foreach (GameObject obj in toRemove)
        {
            playedCardSet.Remove(obj);
        }

        toRemove = null;
    }

    //Show the FrontCard of the MysteryCard => later make animation
    private void DiscoverMysteryWord()
    {
        Destroy(GameObject.Find("MysteryWordMask"));
        float inc = 0;
        foreach (GameObject obj in mysteryCardSet)
        {
            obj.transform.localScale = new Vector3(0.08f, 0.08f, 0);
            obj.transform.position = new Vector3(-1.3f + inc,  0f, obj.transform.position.z);
            inc += 0.3f;
        }
        
    }

}