using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingCard : MonoBehaviour
{
    /********************************* Variables *********************************/

    private PlacedCard myscript;
    private bool placeAvailability = true;

    /********************************* Methods *********************************/
    public bool IsPlaceAvailable()
    {
        return placeAvailability;
    }

    public void SetPlaceAvailability(bool b)
    {
        placeAvailability = b;
    }

    /********************************* Events *********************************/

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Detect Collision1);
        if (other.tag == "PlayedCard")
        {

            //print("A Card is Placed!");
            SetPlaceAvailability(false);
            gameObject.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
            myscript = gameObject.GetComponentInChildren<PlacedCard>();

            //test value of the PlayedCard
            if (other.GetComponent<PlayedCard>().GetValue() == myscript.GetValue())
            {
                myscript.SetWellPlaced(true);
                print("well Placed");

            }
            else
            {
                print("not well placed");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("Not Placed Anymore !");
        SetPlaceAvailability(true);
        gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        myscript = gameObject.GetComponentInChildren<PlacedCard>();
        //myscript.SetPlaced();
        myscript.SetWellPlaced(false);
        print(myscript.IsWellPlaced());

    }

}
