using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitiation : MonoBehaviour
{

    private List<Object> playedCardSet = new List<Object>();
    private List<Object> placedCardSet = new List<Object>();


    // Use this for initialization
    void Start()
    {
        playedCardSet.Add(Resources.Load("Lettre_B"));
        playedCardSet.Add(Resources.Load("Lettre_R"));
        playedCardSet.Add(Resources.Load("Lettre_A"));
        playedCardSet.Add(Resources.Load("Lettre_V"));
        playedCardSet.Add(Resources.Load("Lettre_O"));

        placedCardSet.Add(Resources.Load("PlacedCard"));


        float posX = 0.4f, posY = 0.5f, posZ = 5f;

        for(int i=0; i<playedCardSet.Count; i++)
        {
            Instantiate(playedCardSet[i], new Vector3(posX, posY, posZ), Quaternion.identity);
            posX += 0.2f;
        }

        posX = 0.4f;
        posY = -0.5f;
        posZ = 4f;
        foreach (Object obj in placedCardSet)
        {
            placedCardSet.Add(Instantiate(obj, new Vector3(posX, posY, posZ), Quaternion.identity));
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        //verify if a word is complete

        
        
    }

}

