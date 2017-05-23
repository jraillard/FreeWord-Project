using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedCard : Card
{
    private bool placed=false;

    /********************************* Methods *********************************/



    public bool IsPlaced() //there is a card on the placedCard
    {
        return placed;
    }

    public void SetPlaced()
    {
        if (IsPlaced())
        {
            placed = false;
        }
        else
        {
           placed = true;
        }
    }


    /********************************* Events *********************************/

    /* first might not be usefull 
     * only if the user put the card at the precise 
     * moment where the PlayedCard touch the PlacedCard
    */

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Detect Collision1);
        if (other.tag == "PlayedCard" && IsPlaced()==false)
        {
            //Debug.Log("Placed1");
            other.transform.position = new Vector3(transform.position.x, transform.position.y, 4);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Detect Collision2");
        

        if (other.tag == "PlayedCard" && IsPlaced() == false)
        {
            //Debug.Log("Placed2");
            other.transform.position = new Vector3(transform.position.x, transform.position.y, 4);

        }
    }
}
