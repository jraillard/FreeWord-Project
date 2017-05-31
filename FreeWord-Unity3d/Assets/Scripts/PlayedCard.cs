using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedCard : Card {

    //Card which we use to construct the "PlacedWord"

    /********************************* Variables *********************************/

    private bool visibility = true; //true mean we see the frontCard , false the backCard
    private bool placed = false; //true if we place a PlayedCard on a PlacedCard
    private bool selection = true; //true if we can select this card
    private float distance = 5f; //use for the Drag&Drop management : distance max in (Oz) axis wecan reach with the mouse
    private Vector3 originPlace; //original Place of the card 
    private Vector3 destPlace;  //destination Placed which is set when we put a PlayedCard on a PlacedCard

    /********************************* Loops *********************************/

    private void Start()
    {
        //store the original place of the Card
        originPlace = transform.position;
        //print(originPlace);
    }


    private void FixedUpdate()
    {
        /* loop for testing if there's a PlayedCard forward */

        if(IsVisible() == true && IsPlaced() == false)
        {
            if (Physics.Raycast(transform.position, new Vector3(0, 0, -1), 0.15f))
            {
                SetSelectable(false);
                //print("There is something in front of the object!");
            }
            else
            {
                SetSelectable(true);
                //print("there's nothing");
            }
        }    
    }


    /********************************* Methods *********************************/

    //Get and Set basic methods for placed, selection and visibility
    public bool IsPlaced()
    {
        return placed;
    }

    public void SetPlaced(bool b)
    {
        placed = b;
    }

    public bool IsSelectable()
    {
        return selection;
    }

    public void SetSelectable(bool b)
    {
        selection = b;
    }

    public bool IsVisible()
    {
        return visibility;
    }

    public void SetVisibility(bool b) 
    {
        visibility = b;
    }

    //Receive the position of the ParentObject of the PlacedCard on which we place the PlayedCard
    public void SendPosition(Vector3 pos)
    {
        destPlace = pos;
    }


    /********************************* Events *********************************/

    //Drag
    private void OnMouseDrag()
    {
        if(IsSelectable() == true && IsVisible() == true)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
            //print("Dragging");  
        }
    }

    //Drop
    private void OnMouseUp()
    {
        if (IsPlaced()==true)
        {
            transform.position = destPlace;        
        }else
        {
            transform.position = originPlace;
        }
    }
}