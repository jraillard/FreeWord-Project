using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedCard : Card
{
    /********************************* Variables *********************************/
    private bool wellPlaced = false;

    /********************************* Methods *********************************/

    public bool IsWellPlaced()
    {
        return wellPlaced;
    }

    public void SetWellPlaced(bool b)
    {
        wellPlaced = b;
    }


    /********************************* Events *********************************/

    /* first might not be usefull 
     * only if the user put the card at the precise 
     * moment where the PlayedCard touch the PlacedCard
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayedCard" && GetComponentInParent<PlacingCard>().IsPlaceAvailable() == true)
        {
            other.GetComponent<PlayedCard>().SendPosition(new Vector3(transform.position.x, transform.position.y, 4));
            other.GetComponent<PlayedCard>().SetPlaced(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayedCard" && GetComponentInParent<PlacingCard>().IsPlaceAvailable() == true)
        {
            other.GetComponent<PlayedCard>().SendPosition(new Vector3(transform.position.x, transform.position.y, 4));
            other.GetComponent<PlayedCard>().SetPlaced(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Detect Collision1);
        if (other.tag == "PlayedCard")
        {
            //Debug.Log("Placed1");
            other.GetComponent<PlayedCard>().SetPlaced(false);
        }
    }
}
