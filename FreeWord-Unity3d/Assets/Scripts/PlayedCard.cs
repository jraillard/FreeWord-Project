using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedCard : Card {

    /********************************* Variables *********************************/

    private bool placed = false;
    private bool selection = false;
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

    public void SetSelectable()
    {
        if (IsSelectable())
        {
            selection = false;
        }
        else
        {
            selection = true;
        }
    }

    public void SendPosition(Vector3 pos)
    {
        destPlace = pos;
    }


    /********************************* Events *********************************/

    //DragAndDrop
    private void OnMouseDrag()
    {
        if(IsSelectable())
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
            //print("Dragging");  
        }
        print(IsPlaced());
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