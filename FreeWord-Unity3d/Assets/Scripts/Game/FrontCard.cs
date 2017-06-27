using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCard : MonoBehaviour {

    /*
     * Script to detect a PlayedCard front of a PlayedCard to set the selectability 
     * using the OnTrigger method 
     * not use anymore maybe later ? 
     * 
     */

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PlayedCard")
        {
            print("coucou");
            gameObject.GetComponentInParent<PlayedCard>().SetSelectable(false);
            print(gameObject.GetComponentInParent<PlayedCard>().IsSelectable());
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayedCard")
        {
            print("coucou");
            gameObject.GetComponentInParent<PlayedCard>().SetSelectable(false);
            print(gameObject.GetComponentInParent<PlayedCard>().IsSelectable());
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayedCard")
        {
            print("byebye");
            gameObject.GetComponentInParent<PlayedCard>().SetSelectable(true);
            print(gameObject.GetComponentInParent<PlayedCard>().IsSelectable());
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        }
    }
}
