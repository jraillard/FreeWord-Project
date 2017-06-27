using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool launchLevelDone = false;
    private int cpt; //counter for update loop
    private Data data;
    private List<string> listWord;
    private List<char> splitWord;

    /********************************* Loops *********************************/

    void Start()
    {
        //Get the Word List 
        data = GameObject.Find("DataObject").GetComponent<Data>();
        listWord = data.GetWordListFromGame();

        //Instantiate all the Card Sets

        data.MysteryWord = listWord[0];
        //splitWord = MySplitter(listWord[0]);

        listWord.RemoveAt(0);
        //InitMysteryCardSet(splitWord);
        

        foreach (string s in listWord)
        {
            splitWord = MySplitter(s);
            InitPlayedCardSet(splitWord);
        }

        splitWord = MySplitter(listWord[listWord.Count - 1]);
        InitPlacedCardSet(splitWord);
        InitWordBonusButton();       

    }

    
    // Update is called once per frame    
    void Update()
    {
        /*
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
        {*/
            if (VerifPlacedWord() == true && !launchLevelDone)
            {
                RemovePlacedWord();
                if (playedCardSet.Count == 0 ) //if there's no more PlayedCard 
                {
                    DiscoverMysteryWord();
                }
                else
                {
                    data.AddWordDiscover(listWord[listWord.Count - 1]);
                    listWord.RemoveAt(listWord.Count - 1);
                    splitWord = MySplitter(listWord[listWord.Count - 1]);
                    InitPlacedCardSet(splitWord);
                    InitWordBonusButton();
                }
            }
//        }

//        cpt = 0;
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

    //Instantiate all the MysteryCard to discover => not usefull anymore
    /*
    private void InitMysteryCardSet(List<char> letterList)
    {
        float posX = 0.8f, posY = 0.7f, posZ = 3f;
        int i = 0;

        foreach (char c in letterList)
        {
            //Load the right prefab + sprite  and Instantiate 

            tempObj = Resources.Load("PlayedCard") as GameObject;

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
    */

    //Instantiate all the PlayedCard to discover
    private void InitPlayedCardSet(List<char> letterList)//, float newposY, float newposZ)
    {
        float xTemp = 0f, yTemp = 0.35f, zTemp = 5f; 
        List<float> columnsPos = new List<float>(); //list containing the xPos of the 8 columns of cards
        List<int> cardNbInColumns = new List<int>(); //list containing the number of cards in each column
        List<float> randomList; //list containing the the xPos on cardcolumn with nbCardMin
        int nbCardMin = 0;        
        System.Random r = new System.Random();
        int index = 0;
        int flag = 0;
        int i = playedCardSet.Count;

        columnsPos.Add(0.075f);
        for (int c = 1; c < 8; c++)
        {
            columnsPos.Add(columnsPos[c - 1] + 0.211f);
        }

        for(int c = 0; c<8; c++)
        {
            cardNbInColumns.Add(0);
        }

        for(int j= letterList.Count-1; j>=0; j--)
        {
            //don't do if : space, dash, apostropha
            if (letterList[j] != ' ' && letterList[j] != '-' && letterList[j] != '\'') 
            {
                //Load the right prefab + sprite                  
                tempSprite = Resources.Load("Letter/Letter_" + letterList[j], typeof(Sprite)) as Sprite;
                

                //print(c);
                //print(tempSprite);

                tempObj = Resources.Load("PlayedCard") as GameObject;

                //Find the random position of the card to instantiate 
                foreach (GameObject obj in playedCardSet) //initiate number of card in each column
                {
                    for (int n = 0; n < columnsPos.Count; n++)
                    {
                        if (obj.transform.position.x == columnsPos[n])
                        {
                            cardNbInColumns[n]++;
                        }
                    }
                }

                do
                {
                    flag = 0;
                    randomList = new List<float>();
                    nbCardMin = cardNbInColumns[0]; //by default

                    foreach (int k in cardNbInColumns) //set the minimum cardNumber in each column
                    {
                        if (k <= nbCardMin)
                        {
                            nbCardMin = k;
                        }
                    }

                    for (int n = 0; n < cardNbInColumns.Count; n++) //add the column which have the minimum cardNumber in the randomList
                    {
                        if (cardNbInColumns[n] == nbCardMin)
                        {
                            randomList.Add(columnsPos[n]);
                        }
                    }

                    xTemp = randomList[r.Next(0, randomList.Count)]; //random in this list

                    for (int n = 0; n < columnsPos.Count; n++) //collect the index of the chosen column
                    {
                        if (columnsPos[n] == xTemp)
                        {
                            index = n;
                            break;
                        }
                    }

                    foreach (GameObject obj in playedCardSet)
                    {
                        if (obj.transform.position.x == xTemp)
                        {
                            //same column 
                            if (obj.transform.position.y <= -0.41f) //yMax which is the smaller one (yMax=0.49f but in unity => 0.41999999..) 
                            {
                                yTemp = 0.35f; //must random again
                                columnsPos.RemoveAt(index);
                                cardNbInColumns.RemoveAt(index);
                                flag = -1;
                                break;
                            }

                            if (obj.transform.position.y == 0.35f)
                            {
                                flag = 1;
                            }

                            if (obj.transform.position.y < yTemp) //normal situation
                            {
                                yTemp = obj.transform.position.y;
                                zTemp = obj.transform.position.z;
                            }
                        }
                    }


                    if (flag != -1 || yTemp <= -0.41f) //which means must random againor we're already at the yMax position
                    {
                        if ((yTemp < 0.35f || (yTemp == 0.35f && flag == 1))) // (if not yMin or 1 card in column) and != to yMax
                        {
                            //print(yTemp);
                            yTemp -= 0.07f;
                            zTemp -= 0.1f;
                        }
                    }


                } while (flag < 0); // yTemp doesn't change

                //Instantiate

                playedCardSet.Add(Instantiate(tempObj, new Vector3(xTemp, yTemp, zTemp), Quaternion.identity));

                xTemp = 0f; yTemp = 0.35f; zTemp = 5f; // don't forget to put the initial values
                cardNbInColumns[index]++; //we add on card in this column
                                          //Configure parameters

                //Set the sprite


                playedCardSet[i].GetComponent<SpriteRenderer>().sprite = tempSprite;

                //Set the value of the Card 
                playedCardSet[i].GetComponent<PlayedCard>().SetValue(letterList[j]);
                //print(playedCardSet[i].GetComponent<PlayedCard>().GetValue());
                //print(playedCardSet[i].GetComponent<PlayedCard>().IsVisible() + "2");

                //Make able to DragAndDrop the card
                playedCardSet[i].GetComponent<PlayedCard>().SetSelectable(true);
                playedCardSet[i].GetComponent<PlayedCard>().SetVisibility(true);

                i++;
                //posX += 0.211f;
            }
        }

    }

    //Instantiate all the PlacedCard to discover
    private void InitPlacedCardSet(List<char> letterList)
    {
        float posX = 1.55f, posY = -0.73f, posZ = 2f;
        int i = 0;

        for(int j=letterList.Count-1; j>=0; j--)
        {
            if (letterList[j] == ' ' || letterList[j] == '-' || letterList[j] == '\'')
            {
                //particular case
                switch(letterList[j])
                {
                    case ' ':
                        tempSprite = Resources.Load("Shape/Space", typeof(Sprite)) as Sprite;
                        break;

                    case '-':
                        tempSprite = Resources.Load("Shape/Dash", typeof(Sprite)) as Sprite;
                        break;

                    case '\'':
                        tempSprite = Resources.Load("Shape/Apostrophe", typeof(Sprite)) as Sprite;
                        break;
                }

                //Load the right prefab + sprite  and Instantiate 

                tempObj = Resources.Load("PlacedCard") as GameObject;

                //print(c);
                //print(tempSprite);

                placedCardSet.Add(Instantiate(tempObj, new Vector3(posX, posY, posZ), Quaternion.identity));

                //Configure parameters

                //Set the sprite
                placedCardSet[i].GetComponentInChildren<SpriteRenderer>().sprite = tempSprite;

                //Set the value of the Card 
                placedCardSet[i].GetComponentInChildren<PlacedCard>().SetValue(letterList[j]);

                //Set it placed
                placedCardSet[i].GetComponentInChildren<PlacedCard>().SetWellPlaced(true);
                

                //print(placedCardSet[i].GetComponentInChildren<PlacedCard>().GetValue());

                //placedCardSet[i].GetComponentInChildren<SpriteRenderer>().color = Color.white;

                //Make the card not Hidden (shown only with WordBonus)
                placedCardSet[i].GetComponentInChildren<PlacedCard>().SetVisibility(true);

                i++;
                posX -= 0.211f;
            }
            else
            {
                //Load the right prefab + sprite  and Instantiate 

                tempObj = Resources.Load("PlacedCard") as GameObject;


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
            //print("Great!");
            return true;
        }
        else
        {
            //print("Not a word!");
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
        if(!launchLevelDone)
        {
            data.AddWordDiscover(listWord[0]); // add the last word 
            print("coucou");
            SceneManager.LoadSceneAsync("LevelDone", LoadSceneMode.Single);
            launchLevelDone = true;
        }
          
    }

}