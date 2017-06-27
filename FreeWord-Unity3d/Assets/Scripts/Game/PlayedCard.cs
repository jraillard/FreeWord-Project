using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedCard : Card {

    //Card which we use to construct the "PlacedWord"

    /********************************* Variables *********************************/

    private bool placed; //true if we place a PlayedCard on a PlacedCard
    private bool selection = true; //true if we can select this card
    private float distance = 3f; //use for the Drag&Drop management : distance max in (Oz) axis wecan reach with the mouse
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
        //loop for testing if there's a PlayedCard forward

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

    //Get and Set basic methods for placed, selection 
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

    //Receive the position of the ParentObject of the PlacedCard on which we place the PlayedCard
    public void SendPosition(Vector3 pos)
    {
        destPlace = pos;
    }

    //function use when user Drop a PlayedCard but not on a PlacedCard
    public void Drop(float yPas, float zPas)
    {
        int flag = 0;
        float yMin = 0, zMin = 0;
        List<GameObject> otherCards = GameObject.Find("BackGround").GetComponent<GameManagement>().GetCardSet("played");

        foreach (GameObject obj in otherCards)
        {
            if (obj.transform.position.x == originPlace.x)
            {
                if (flag == 0)
                {
                    yMin = obj.transform.position.y;
                    zMin = obj.transform.position.z;
                    flag = 1;
                }
                else if (obj.transform.position.y < yMin)
                {
                    yMin = obj.transform.position.y;
                    zMin = obj.transform.position.z;
                }
            }

        }

        if (flag == 0) //there's no more cards in the playedCardSet
        {
            yMin = originPlace.y;
            zMin = originPlace.z;
        }
        else
        {
            yMin -= yPas;
            zMin -= zPas;
        }

        transform.position = new Vector3(originPlace.x, yMin, zMin);
    }


    /********************************* Events *********************************/

    //Drag
    private void OnMouseDrag()
    {
        if(IsSelectable() == true && IsVisible() == true)
        {
            //transform.position.z = distance [camera;card]
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
            if (transform.position != originPlace)
            {
                Drop(0.08f, 0.1f);
            }
        }
    }

}