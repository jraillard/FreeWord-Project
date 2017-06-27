using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Username : MonoBehaviour {

    public GameObject salut;

    private string[] Lines;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Lines = System.IO.File.ReadAllLines(Application.persistentDataPath + "/ID/JA.txt");
        salut.GetComponent<Text>().text = "Hello " + Lines[0];
    }
}
