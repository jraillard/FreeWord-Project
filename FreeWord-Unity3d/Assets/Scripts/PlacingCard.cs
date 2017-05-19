using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingCard : MonoBehaviour {

    /* first might not be usefull 
     * only if the user put the card at the precise 
     * moment where the PlayedCard touch the PlacedCard
    */
    
    private void OnTriggerEnter(Collider other) 
    {
        //Debug.Log("Detect Collision1);

        if (other.tag == "PlayedCard")
        {
            //Debug.Log("Placed1");
            other.transform.position = new Vector3(transform.position.x, transform.position.y, 4);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Detect Collision2");

        if (other.tag == "PlayedCard")
        {
            //Debug.Log("Placed2");
            other.transform.position = new Vector3(transform.position.x, transform.position.y, 4);
        }
    }
    
}
