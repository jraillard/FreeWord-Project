using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour {

    private GameObject obj1;
    private GameObject obj2;

    private void Start()
    {
        obj1 = GameObject.Find("CategorySelected_1");
        obj2 = GameObject.Find("CategorySelected_2");
    }

    public void SetCategorySelected_1()
    {         
       obj1.GetComponent<Text>().text = this.transform.Find("Text_Up").GetComponent<Text>().text + " : Level 0"; 
    }

    public void SetCategorySelected_2()
    {
       obj2.GetComponent<Text>().text = this.transform.Find("Text_Down").GetComponent<Text>().text;
    }
}
