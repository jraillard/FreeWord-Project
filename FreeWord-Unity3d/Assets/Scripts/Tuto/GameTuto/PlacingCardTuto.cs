using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingCardTuto: MonoBehaviour
{
    /* 
     * Script put on the ParentObject from the PlayedCard 
     * it just contain a collider 
     * when PlayedCard is drop on a PlacedCard => it is place 
     * if this ParentObject
    */

    /********************************* Variables *********************************/

    private PlacedCardTuto myscript;
    private bool placeAvailability = true; //true if there's no PlayedCard on the ParentObject

    /********************************* Methods *********************************/

    //Get and Set basic methods for placeAvailability
    public bool IsPlaceAvailable()
    {
        return placeAvailability;
    }

    public void SetPlaceAvailability(bool b)
    {
        placeAvailability = b;
    }

    /********************************* Events *********************************/

    //Manage the placement(collision) of a PlayedCard on the ParentObject of the PlacedCard
    private void OnTriggerEnter(Collider other) //PlayedCard Collider enter in the ParentObject Collider
    {
        //Debug.Log("Detect Collision1");
        if (other.tag == "PlayedCard")
        {

            //print("A Card is Placed!");
            SetPlaceAvailability(false);
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0); =>can be set à 0 always work perfectly
            myscript = gameObject.GetComponentInChildren<PlacedCardTuto>();

            //test value of the PlayedCard
            if (other.GetComponent<PlayedCardTuto>().GetValue() == myscript.GetValue())
            {
                myscript.SetWellPlaced(true);
                //print("well Placed");

            }
            else
            {
                //print("not well placed");
            }

            //Allow placed attributes of PlayedCard to be true while it's placed
            other.GetComponent<PlayedCardTuto>().SetPlaced(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
            //print("Not Placed Anymore !");
            SetPlaceAvailability(true);
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
            myscript = gameObject.GetComponentInChildren<PlacedCardTuto>();
            //myscript.SetPlaced();
            myscript.SetWellPlaced(false);
            //print(myscript.IsWellPlaced());

            //Allow placed attributes of PlayedCard to be true while it's placed
            other.GetComponent<PlayedCardTuto>().SetPlaced(false);
    } 
}
