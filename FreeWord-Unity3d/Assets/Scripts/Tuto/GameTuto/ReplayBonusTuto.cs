﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayBonusTuto : MonoBehaviour {
    //script for the Replaybonus => replace the Playedcard where there's place (using Drop() from PlayedCard)

    /********************************* Variables *********************************/

    private List<GameObject> tempPlayedCardSet; //playedCardSet loaded
    private PlayedCardTuto objScript;//playedCard script loaded

    /********************************* Methods *********************************/

    public void ReplacePlayedCard()
    { 
        tempPlayedCardSet = GameObject.Find("BackGround").GetComponent<GameManagementTuto>().GetCardSet("played");

        foreach (GameObject obj in tempPlayedCardSet)
        {
            //print(obj.ToString());            
            objScript = obj.GetComponent<PlayedCardTuto>();
            
            if (objScript.IsPlaced() == true)
            {
                //print("drop");
                objScript.Drop(0.08f, 0.1f);
            }                 
        }    
    }

}
