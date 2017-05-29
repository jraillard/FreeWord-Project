using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedCard : Card {

    /********************************* Variables *********************************/
    private bool visibility = true; 
    private bool placed = false;
    private bool selection = true;
    private float distance = 5f;
    private Vector3 originPlace;
    private Vector3 destPlace;

    /********************************* Loops *********************************/

    private void Start()
    {
        //store the original place
        originPlace = transform.position;
        //print(originPlace);
    }


    private void FixedUpdate()
    {
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
    public bool IsPlaced() //there is a card on the placedCard
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

    public void SendPosition(Vector3 pos)
    {
        destPlace = pos;
    }

    public bool IsVisible()
    {
        return visibility;
    }

    public void SetVisibility(bool b) //change the visibility on an event
    {
        visibility = b;
    }


    /********************************* Events *********************************/

    //DragAndDrop
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