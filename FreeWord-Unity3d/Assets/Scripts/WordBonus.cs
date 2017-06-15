using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBonus : MonoBehaviour
{

    /********************************* Variables *********************************/

    private List<GameObject> tempPlacedCards; //placedCardSet loaded
    private List<int> indexList; //use to store the index created by the random number to choose the letters to show 
    private PlacedCard currentCardScript; //used to apply method on the letter we want to show
    private int index; //store the random number
    private int nbLetterShown; 
    private bool indexUsed; //flag for index  
    private bool bonusAvailable = true; //true :  user can you the button
    private Sprite tempSprite;
    private char tempLetter;

    /********************************* Loops *********************************/

    //needed for test not for the game properly 
    /*  
    private void Update()
    {
        print("bonus availability" + IsBonusAvailable());
    }
    */

    /********************************* Methods *********************************/

    //Get and Set basic methods for bonusAvailable 
    public bool IsBonusAvailable()
    {
        return bonusAvailable;
    }

    public void SetBonusAvailability(bool b)
    {
        bonusAvailable = b;
    }

    //Show 1/3 of the PlacedCard Letters
    public void ShowSomeLetter()
    {
        if (IsBonusAvailable() == true)
        {
            nbLetterShown = 0;
            index = 0;
            indexList = new List<int>();
            tempPlacedCards = GameObject.Find("BackGround").GetComponent<GameManagement>().GetCardSet("placed");

            //We Show Letter only if there's no PlacedCards
            //mustn't change availability in Update!!! 
            //or we we'll need second variable to set bonusavailability

            foreach (GameObject obj in tempPlacedCards)
            {
                if (obj.GetComponent<PlacingCard>().IsPlaceAvailable() == false)
                {
                    print("No card must be placed to use this bonus !");
                    return;
                }
            }

            do
            {
                //index choice 
                do
                {
                    indexUsed = false;
                    index = (int)UnityEngine.Random.Range(0f, (float)tempPlacedCards.Count); //conflict between Unity and MSDN Random class => must specify

                    foreach (int i in indexList) //verify if index not used
                    {
                        if (index == i)
                        {
                            indexUsed = true;
                            break;
                        }
                    }

                } while (indexUsed != false);


                currentCardScript = tempPlacedCards[index].GetComponentInChildren<PlacedCard>(); //load the card script
                //print(currentCardScript.ToString());
                //print(currentCardScript.IsVisible());
                //Console.ReadLine();

                //if not visible 
                if (currentCardScript.IsVisible() == false)
                {
                    //Showing process
                    tempLetter = tempPlacedCards[index].GetComponentInChildren<PlacedCard>().GetValue();
                    tempSprite = Resources.Load("Shape/Shape_" + tempLetter, typeof(Sprite)) as Sprite;
                    tempPlacedCards[index].GetComponentInChildren<SpriteRenderer>().sprite= tempSprite;
                    currentCardScript.SetVisibility(true);
                    indexList.Add(index);
                    nbLetterShown++;
                    //print(tempPlacedCards.Count);
                }

            } while (nbLetterShown < Math.Ceiling(((double)tempPlacedCards.Count) / 3));

            //Free data mem
            currentCardScript = null;
            indexList = null;
            tempPlacedCards = null;

            //Bonus not available anymore 
            SetBonusAvailability(false);
        }
    }
}
