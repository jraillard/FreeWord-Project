using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    /********************************* Variables *********************************/

    //must be GameObject to be used after
    private List<GameObject> mysteryCardSet = new List<GameObject>();
    private List<GameObject> playedCardSet = new List<GameObject>();
    private List<GameObject> placedCardSet = new List<GameObject>();

    //need (almost) to store Prefab before Instantiate 
    private GameObject tempObj;

    private int cpt; 
    private List<char> splitWord;

    /********************************* Loops *********************************/

    void Start()
    {

        splitWord = MySplitter("BRAVO");

        InitMysteryCardSet(splitWord);
        InitPlayedCardSet(splitWord);
        InitPlacedCardSet(splitWord);

    }

    // Update is called once per frame
    
    void Update()
    {
        //verify if a word is complete
        foreach(GameObject obj in placedCardSet)
        {
            if (obj.GetComponent<PlacingCard>().IsPlaceAvailable()==false)
            {
                cpt++;
            }
        }
        //print(cpt);
        if(cpt==placedCardSet.Count)
        {
            if(VerifPlacedWord()==true)
            {
                RemovePlacedWord();
                if(playedCardSet.Count==0)
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

    private List<string> ReceivePlayedWords()
    {
        // here will be written instruction to the webservice 
        return null;
    }

    private List<char> MySplitter(string word)
    {
        List<char> splitWord = new List<char>();
        for (int j = 0; j < word.Length; j++)
        {
            splitWord.Add(word[j]);
        }

        return splitWord;
    }

    private void InitMysteryCardSet(List<char> letterList)
    {
        float posX = 0.4f, posY = 0.5f, posZ = 5f;
        int i = 0;

        foreach (char c in letterList)
        {
            //Load the right prefab  and Instantiate 
            tempObj = Resources.Load("PlayedLetter/Letter_" + c) as GameObject;
            mysteryCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the value of the Card 
            mysteryCardSet[i].GetComponent<PlayedCard>().SetValue(c);
            //print(mysteryCardSet[i].GetComponent<PlayedCard>().GetValue());

            //Make the PlacedCard hidden
            mysteryCardSet[i].GetComponent<PlayedCard>().SetVisibility();
            //print(mysteryCardSet[i].GetComponent<PlayedCard>().IsVisible()+"1");
            mysteryCardSet[i].GetComponentInChildren<SpriteRenderer>().color = Color.black;


            i++;
            posX += 0.3f;
        }

    }

    private void InitPlayedCardSet(List<char> letterList)
    {
        float posX = 0.4f, posY = 0f, posZ = 5f;
        int i = 0;

        foreach (char c in letterList)
        {

            //Load the right prefab and Instantiate
            tempObj = Resources.Load("PlayedLetter/Letter_" + c) as GameObject;
            playedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the value of the Card 
            playedCardSet[i].GetComponent<PlayedCard>().SetValue(c);
            //print(playedCardSet[i].GetComponent<PlayedCard>().GetValue());
            //print(playedCardSet[i].GetComponent<PlayedCard>().IsVisible() + "2");

            //Make able to DragAndDrop the card
            playedCardSet[i].GetComponent<PlayedCard>().SetSelectable();

            i++;
            posX += 0.3f;

        }
    }

    private void InitPlacedCardSet(List<char> letterList)
    {
        float posX = 0f, posY = -0.5f, posZ = 4f;
        int i = 0;

        foreach (char c in letterList)
        {
            //Load the right prefab and instantiate
            tempObj = Resources.Load("PlacedLetter/Letter_" + c) as GameObject;
            placedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the value of the Card 
            placedCardSet[i].GetComponentInChildren<PlacedCard>().SetValue(c);
            //print(placedCardSet[i].GetComponentInChildren<PlacedCard>().GetValue());

            
            //Make the PlacedCard hidden
            placedCardSet[i].GetComponentInChildren<PlacedCard>().SetVisibility();
            //print(placedCardSet[i].GetComponentInChildren<PlacedCard>().IsVisible() + "3");
            
            i++;
            posX += 0.3f;
        }
    }

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

    private void DiscoverMysteryWord()
    {
        foreach(GameObject obj in mysteryCardSet)
        {
            obj.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

}