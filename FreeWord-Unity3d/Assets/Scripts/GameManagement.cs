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

    private int cpt; //counter for update loop
    private List<char> splitWord;

    /********************************* Loops *********************************/

    void Start()
    {
        //Initiate the game 

        //Receive the letter we need
        splitWord = MySplitter("BRAVO");

        //Instantiate all the Card Sets
        InitMysteryCardSet(splitWord);
        InitPlayedCardSet(splitWord);
        InitPlacedCardSet(splitWord);

    }

    // Update is called once per frame    
    void Update()
    {
        //verify if every PlacedCard have a PlayedCard on it 
        foreach(GameObject obj in placedCardSet)
        {
            if (obj.GetComponent<PlacingCard>().IsPlaceAvailable()==false)
            {
                cpt++;
            }
        }
        //print(cpt);

        if(cpt==placedCardSet.Count) //if it is 
        {
            if(VerifPlacedWord()==true) 
            {
                RemovePlacedWord();
                if(playedCardSet.Count==0) //if there's no more PlayedCard 
                {
                    DiscoverMysteryWord();
                }
                else
                {
                    InitPlacedCardSet(splitWord);
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

    //Instantiate all the MysteryCard to discover
    private void InitMysteryCardSet(List<char> letterList)
    {
        float posX = 0.4f, posY = 0.7f, posZ = 5f;
        int i = 0;

        foreach (char c in letterList)
        {
            //Load the right prefab  and Instantiate 
            if (c >= 'a' && c <= 'z')
            {
                //minuscule
                tempObj = Resources.Load("PlayedLetter/Letter_" + c) as GameObject;
            }
            else if (c >= 'A' && c <= 'Z')
            {
                //majuscule
                tempObj = Resources.Load("PlayedLetter/M_Letter_" + c) as GameObject;
            }

            mysteryCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the value of the Card 
            mysteryCardSet[i].GetComponent<PlayedCard>().SetValue(c);
            //print(mysteryCardSet[i].GetComponent<PlayedCard>().GetValue());

            //Make the PlacedCard hidden
            mysteryCardSet[i].GetComponent<PlayedCard>().SetSelectable(false);
            mysteryCardSet[i].GetComponent<PlayedCard>().SetVisibility(false);
            //print(mysteryCardSet[i].GetComponent<PlayedCard>().IsVisible()+"1");
            mysteryCardSet[i].GetComponent<SpriteRenderer>().color = Color.black;


            i++;
            posX += 0.3f;
        }

    }

    //Instantiate all the PlayedCard to discover
    private void InitPlayedCardSet(List<char> letterList)
    {
        float posX = 0.4f, posY = 0.2f, posZ = 5f;
        int i = 0;

        for(int j=0; j<6; j++)
        {
            foreach (char c in letterList)
            {

                //Load the right prefab and Instantiate
                if(c>='a' && c<='z')
                {
                    //minuscule
                    tempObj = Resources.Load("PlayedLetter/Letter_" + c) as GameObject;
                }
                else if(c>='A' && c<='Z')
                {
                    //majuscule
                    tempObj = Resources.Load("PlayedLetter/M_Letter_" + c) as GameObject;
                }
                
                playedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

                //Configure parameters

                //Set the value of the Card 
                playedCardSet[i].GetComponent<PlayedCard>().SetValue(c);
                //print(playedCardSet[i].GetComponent<PlayedCard>().GetValue());
                //print(playedCardSet[i].GetComponent<PlayedCard>().IsVisible() + "2");

                //Make able to DragAndDrop the card
                playedCardSet[i].GetComponent<PlayedCard>().SetSelectable(true);
                playedCardSet[i].GetComponent<PlayedCard>().SetVisibility(true);

                i++;
                posX += 0.3f;

            }

            posX = 0.4f;
            posY -= 0.08f;
            posZ -= 0.1f;
        }
        
    }

    //Instantiate all the PlacedCard to discover
    private void InitPlacedCardSet(List<char> letterList)
    {
        float posX = 0f, posY = -0.7f, posZ = 4f;
        int i = 0;

        foreach (char c in letterList)
        {
            //Load the right prefab and instantiate
            if (c >= 'a' && c <= 'z')
            {
                //minuscule
                tempObj = Resources.Load("PlacedLetter/Letter_" + c) as GameObject;
            }
            else if (c >= 'A' && c <= 'Z')
            {
                //majuscule
                tempObj = Resources.Load("PlacedLetter/M_Letter_" + c) as GameObject;
            }

            placedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the value of the Card 
            placedCardSet[i].GetComponentInChildren<PlacedCard>().SetValue(c);
            //print(placedCardSet[i].GetComponentInChildren<PlacedCard>().GetValue());

            //placedCardSet[i].GetComponentInChildren<SpriteRenderer>().color = Color.black;

            i++;
            posX += 0.3f;
        }
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
        foreach(GameObject obj in placedCardSet)
        {
            toRemove.Add(obj);
            Destroy(obj); 
        }

        foreach(GameObject obj in toRemove)
        {            
            placedCardSet.Remove(obj);
        }


        toRemove = new List<GameObject>(); //DON'T FORGET !!!
        //Remove PlayedCard wellPlaced 

        foreach (GameObject obj in playedCardSet)
        {
            if (obj.GetComponent<Transform>().position.z == 4)
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

    //Show the FrontCard of the MysteryCard
    private void DiscoverMysteryWord()
    {
        foreach(GameObject obj in mysteryCardSet)
        {
            obj.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

}