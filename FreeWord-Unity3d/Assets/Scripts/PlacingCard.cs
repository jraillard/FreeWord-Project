using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingCard : MonoBehaviour
{

    /* first might not be usefull 
     * only if the user put the card at the precise 
     * moment where the PlayedCard touch the PlacedCard
    */


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Detect Collision1);
        if (other.tag == "PlayedCard" )
        {
            print("A Card is Placed!");            
            gameObject.GetComponent<BoxCollider>().size = new Vector3(0,0,0);            
            PlacedCard myscript = gameObject.GetComponentInChildren<PlacedCard>(); 
            myscript.SetPlaced();
        }
    }

    private void OnTriggerExit(Collider other)
    {
      
            print("Not Placed Anymore !");
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
            PlacedCard myscript = gameObject.GetComponentInChildren<PlacedCard>();
            myscript.SetPlaced();
        
    }


}
