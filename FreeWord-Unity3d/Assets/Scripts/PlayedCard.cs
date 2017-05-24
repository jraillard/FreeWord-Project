using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedCard : Card {

    private bool selection = false;
    private float distance = 5f;
    private Vector3 originPlace;

    /********************************* Methods *********************************/

    private void Start()
    {
        //store the original place
        originPlace = transform.position;
        //print(originPlace);

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
    }

    private void OnMouseUp()
    {
        
        if (transform.position.z == 5)
        {
            //== 4 if we're on a PlacedCard 

            //print("Replacing");
            transform.position = originPlace;
        }
    }
}

