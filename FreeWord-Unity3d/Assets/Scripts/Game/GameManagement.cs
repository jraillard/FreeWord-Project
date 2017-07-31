using System;
using System.Collections.Generic;
using System.Linq;
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
    private ImageDownLoader image;
    private Dictionary<string, int> listWord = new Dictionary<string, int>();
    private List<string> wordUrlList = new List<string> ();
    private List<char> splitWord;
    private bool firstpic = false;

    /********************************* Loops *********************************/

    void Start()
    {
        //Get the Word List 
        data = GameObject.Find("DataObject").GetComponent<Data>();
        image = GameObject.Find("Image").GetComponent<ImageDownLoader>();

        //InitWordList();

        Dictionary<string, int> tempListWord = data.GetWordListFromCategory();
        List<string> tempUrlList = data.GetWordUrlListFromCategory();

        //Separate possible mysteryword and word playable (if not level0)
        if (data.CurrentLevel == 0)
        {
            print("coucou");
            int i = 0;
            //implement wordlist
            foreach (KeyValuePair<string, int> k in tempListWord)
            {
                if (i == 6)
                {
                    break;
                }
                listWord.Add(k.Key, k.Value);
                i++;
            }

            print(listWord.Count);
            //implement urlwordlist
            for (int k = 0; k < 6; k++)
            {
                wordUrlList.Add(tempUrlList[k]);
            }
            //setmysteryword
            data.MysteryWord = listWord.Keys.Last();
            data.MysteryWordUrl = wordUrlList[5];
            listWord.Remove(data.MysteryWord);
            wordUrlList.RemoveAt(5);
        }
        else
        {
            Dictionary<string, string> shuffledList = new Dictionary<string, string>();
            Dictionary<string, int> playableWordList = new Dictionary<string, int>();
            Dictionary<string, string> playableWordUrlList = new Dictionary<string, string>();

            bool mysteryWordFind = false;

            int idx=0;
            foreach (KeyValuePair<string, int> k in tempListWord)
            {
                if (k.Value == 0 && mysteryWordFind == false) //just need one mysteryword
                {
                    //Set mysteryWord
                    data.MysteryWord = k.Key;
                    data.MysteryWordUrl = tempUrlList[idx];

                    mysteryWordFind = true;
                }
                else if(k.Value > 0)
                {

                    playableWordList.Add(k.Key, k.Value);
                    playableWordUrlList.Add(k.Key, tempUrlList[idx]);
                }
                idx++; //no indexor in dictionary
            }

            //Add five random playword to the list
            int nbword = 0;
            int nbtime = 1; //nbtime discovered
            int i = 0; 
            int j = 0;
            bool flag = false;
            //bool flagFirstdTemp = false;
            //int r = 0;
            System.Random r = new System.Random();
            //int nbWordInStep;
            int nbWordChosenInStep;
            Dictionary<string, int> dTemp = new Dictionary<string, int>();
            List<int> index;
            //List<string> keys = new List<string>();

            do
            {
                index = new List<int>();
                dTemp = new Dictionary<string, int>();
                //nbWordInStep = playableWordList.Where(p => p.Value == nbtime).ToList().Count;
                nbWordChosenInStep = 0;

                //fill the first temporary list
                do
                {
                    dTemp = playableWordList.Where(p => p.Value == nbtime).ToDictionary(p => p.Key, p => p.Value);
                    nbtime++;
                } while (dTemp.Count <= 0);

                nbtime--; //we finish the loop after the right step               

                do
                {
                    i = 0;
                    //choose index randomly                    
                    do
                    {
                        j = -1;
                        j = r.Next(0, dTemp.Count);
                        if (!index.Contains(j))
                        {
                            index.Add(j);
                        }
                        else
                        {
                            j = -1;
                        }

                    } while (j == -1);
                    
                    //string key = "";                  

                    foreach (KeyValuePair<string, int> k in dTemp)
                    {
                        if (i == j)
                        {
                            string x = playableWordUrlList.Where(p => p.Key == k.Key).Select(p => p.Value).First();
                            shuffledList.Add(k.Key, k.Value  + "|" + x);
                            //key = k.Key;
                            //keys.Add(k.Key);
                            flag = true;
                            break;
                        }
                        i++;
                    }
                    //print(playableWordList.ContainsKey(key));
                    if(flag == true)
                    {
                        //playableWordList.Remove(key);
                        //playableWordUrlList.Remove(key);
                        nbword++;
                        nbWordChosenInStep++;
                        flag = false;
                    }
                    else
                    {
                        break;
                    }                   
                    
                 } while (nbword < 5 && nbWordChosenInStep < dTemp.Count);

                nbtime++; //go to next step              

            } while (nbword < 5);

            playableWordList = null;
            playableWordUrlList = null;

            //shuffle the list
            System.Random rand = new System.Random();
            shuffledList = shuffledList.OrderBy(x => rand.Next()).ToDictionary(item => item.Key, item => item.Value);

            //split lists
            foreach(KeyValuePair<string, string> k in shuffledList)
            {
                listWord.Add(k.Key, int.Parse(k.Value.Split('|')[0]));
                wordUrlList.Add(shuffledList.Where(p => p.Key == k.Key).Select(p => p.Value).First().Split('|')[1]);
            }

        }

        //Send playedwordlist to Data
        data.SetWordListPlayed(listWord);
        print(listWord.Count);

        //Init the CardSet
        //print(wordUrlList.Count());
        foreach (KeyValuePair<string, int> k in listWord)
        {
            splitWord = MySplitter(k.Key);
            InitPlayedCardSet(splitWord);
        }

        splitWord = MySplitter(listWord.Keys.Last());
        InitPlacedCardSet(splitWord, wordUrlList.Last());
        InitWordBonusButton();
        //StartCoroutine(image.LoadImage(listWord.Keys.Last(), wordUrlList.Last(), data.CurrentCatName));

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
            if(firstpic==false)
            {
                StartCoroutine(image.LoadImage(listWord.Keys.Last(), wordUrlList.Last(), data.CurrentCatName));
                firstpic = true;
            }
        
            if (VerifPlacedWord() == true && !launchLevelDone)
            {
                RemovePlacedWord();
                if (playedCardSet.Count == 0 ) //if there's no more PlayedCard 
                {
                    DiscoverMysteryWord();
                }
                else
                {
                    listWord.Remove(listWord.Keys.Last());
                    wordUrlList.Remove(wordUrlList.Last());
                    StartCoroutine(image.LoadImage(listWord.Keys.Last(), wordUrlList.Last(), data.CurrentCatName));
                    splitWord = MySplitter(listWord.Keys.Last());
                    InitPlacedCardSet(splitWord,wordUrlList.Last());
                    
                    InitWordBonusButton();
                }
            }
            
//        }

//        cpt = 0;
    }    

    /********************************* Methods *********************************/

    public void InitWordList()
    {
        Dictionary<string, int> tempListWord = data.GetWordListFromCategory();
        List<string> tempUrlList = data.GetWordUrlListFromCategory();

        //Separate possible mysteryword and word playable (if not level0)
        if (data.CurrentLevel == 0)
        {
            print("coucou");
            int i = 0;
            //implement wordlist
            foreach (KeyValuePair<string, int> k in tempListWord)
            {
                if (i == 6) { break; }
                listWord.Add(k.Key, k.Value);
                i++;
            }

            print(listWord.Count);
            //implement urlwordlist
            for (int k = 0; k < 6; k++)
            {
                wordUrlList.Add(tempUrlList[k]);
            }
            //setmysteryword
            data.MysteryWord = listWord.Keys.First();
            listWord.Remove(data.MysteryWord);


        }
        else
        {
            Dictionary<string, int> mysteryWordList = new Dictionary<string, int>();
            Dictionary<string, int> playableWordList = new Dictionary<string, int>();
            List<string> playableWordUrlList = new List<string>();

            bool mysteryWordFind = false;

            //print(tempListWord["abricot"]);
            //print(tempListWord[tempListWord.Keys.First()] + " | " + tempListWord.Keys.First());

            foreach (KeyValuePair<string, int> k in tempListWord)
            {
                if (k.Value == 0 && mysteryWordFind == false) //just need one 
                {
                    print(k.Key);
                    print(tempUrlList[tempListWord[k.Key] - 1]);
                    mysteryWordList.Add(k.Key, k.Value);
                    wordUrlList.Add(tempUrlList[tempListWord[k.Key]]);
                    mysteryWordFind = true;
                }
                else
                {
                    playableWordList.Add(k.Key, k.Value);
                    playableWordUrlList.Add(tempUrlList[tempListWord[k.Key]]);
                }
            }
            //Add five random playword to the list
            int nb = 0;
            int i;
            System.Random r = new System.Random();
            do
            {
                i = 0;
                string key = "";
                int j = r.Next(0, playableWordList.Count);

                foreach (KeyValuePair<string, int> k in playableWordList)
                {
                    if (i == j)
                    {
                        listWord.Add(k.Key, k.Value);
                        wordUrlList.Add(playableWordUrlList[j]);
                        key = k.Key;
                        break;
                    }
                    i++;
                }
                //print(playableWordList.ContainsKey(key));
                playableWordList.Remove(key);
                playableWordUrlList.RemoveAt(j);

                nb++;

            } while (nb < 5);

            //Set mysteryWord
            data.MysteryWord = mysteryWordList.Keys.First();
            //print(data.MysteryWord);
            //mysterywordurl is in wordurlList[0]
        }

    }


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
    private void InitPlacedCardSet(List<char> letterList, string url)
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
                /*print("******"+placedCardSet[i].GetComponentInChildren<PlacedCard>().GetValue()+"*****");
                print("****" + letterList[j] + "*****");*/

                //Set it placed
                placedCardSet[i].GetComponentInChildren<PlacedCard>().SetWellPlaced(true);
                placedCardSet[i].GetComponent<PlacingCard>().SetPlaceAvailability(false);


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
            print("coucou");
            SceneManager.LoadSceneAsync("LevelDone", LoadSceneMode.Single);
            launchLevelDone = true;
        }
          
    }

}