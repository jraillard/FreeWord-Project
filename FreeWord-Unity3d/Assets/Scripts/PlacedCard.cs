using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedCard : Card
{
    //Card which, together, build the PlacedWord

    /********************************* Variables *********************************/
    private bool wellPlaced = false; //true if the PlayedCard placed and PlacedCard have the same value

    /********************************* Methods *********************************/
    
    //Get and Set basic methods for wellPlaced
    public bool IsWellPlaced()
    {
        return wellPlaced;
    }

    public void SetWellPlaced(bool b)
    {
        wellPlaced = b;
    }


    /********************************* Events *********************************/

    //Manage the placement(collision) of a PlayedCard on a PlacedCard
    private void OnTriggerEnter(Collider other) //PlayedCard Collider enter in the PlacedCard Collider
    {
        if (other.tag == "PlayedCard" && GetComponentInParent<PlacingCard>().IsPlaceAvailable() == true)
        { 
            other.GetComponent<PlayedCard>().SendPosition(new Vector3(transform.position.x, transform.position.y, 4));
            other.GetComponent<PlayedCard>().SetPlaced(true);
        }
    }

    private void OnTriggerStay(Collider other)//PlayedCard Collider stay in the PlacedCard Collider
    {
        if (other.tag == "PlayedCard" && GetComponentInParent<PlacingCard>().IsPlaceAvailable() == true)
        {
            other.GetComponent<PlayedCard>().SendPosition(new Vector3(transform.position.x, transform.position.y, 4));
            other.GetComponent<PlayedCard>().SetPlaced(true);
        }
    }

    private void OnTriggerExit(Collider other) //PlayedCard Collider exit from the PlacedCard Collider
    {
        //Debug.Log("Detect Collision1);
        if (other.tag == "PlayedCard")
        {
            //Debug.Log("Placed1");
            other.GetComponent<PlayedCard>().SetPlaced(false);
        }
    }

}
