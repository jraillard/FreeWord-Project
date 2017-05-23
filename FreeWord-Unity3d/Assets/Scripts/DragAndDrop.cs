using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

    private float distance = 5f;
    private Vector3 originPlace;

    private void Start()
    {
        //store the original place
        originPlace = transform.position;
        print(originPlace);
        
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition= new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
        //print("Dragging");  
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
