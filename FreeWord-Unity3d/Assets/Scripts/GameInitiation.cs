using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitiation : MonoBehaviour
{
    //must be GameObject to be used after
    private List<GameObject> mysteryCardSet = new List<GameObject>();
    private List<GameObject> playedCardSet = new List<GameObject>();
    private List<GameObject> placedCardSet = new List<GameObject>();

    //need (almost) to store Prefab before Instantiate 
    private GameObject tempObj; 

    // Use this for initialization
    void Start()
    {
        List<char> splitWord;

        splitWord = MySplitter("BRAVO");
       
        InitMysteryCardSet(splitWord);
        InitPlayedCardSet(splitWord);
        InitPlacedCardSet(splitWord);

    }



    // Update is called once per frame
    void Update()
    {
        //verify if a word is complete



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
            //print(ListWord[j]);
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
            //tempObj = Resources.Load("PlayedLetter/Letter_" + c) as GameObject;
            mysteryCardSet.Add(Instantiate(Resources.Load("PlayedLetter/Letter_" + c) as GameObject, new Vector3(posX, posY, posZ), Quaternion.identity));

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
       float  posX = 0f, posY = -0.5f, posZ = 4f;
       int i = 0;

        foreach (char c in letterList)
        {
            //Load the right prefab and instantiate
            tempObj = Resources.Load("PlacedLetter/Letter_" + c) as GameObject;
            placedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

            //Configure parameters

            //Set the value of the Card 
            placedCardSet[i].GetComponentInChildren<PlacedCard>().SetValue(c);
            print(placedCardSet[i].GetComponentInChildren<PlacedCard>().GetValue());

            //Make the PlacedCard hidden
            placedCardSet[i].GetComponentInChildren<PlacedCard>().SetVisibility();
            //print(placedCardSet[i].GetComponentInChildren<PlacedCard>().IsVisible() + "3");
            placedCardSet[i].GetComponentInChildren<SpriteRenderer>().color = Color.red;

            i++;
            posX += 0.3f;
        }
    }

}

