using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndTuto : MonoBehaviour
{

    public GameObject congrats;
    public GameObject word;
    public GameObject next;

    void Start()
    {
        StartCoroutine(Time());
        //StartCoroutine(Time());
        //FC.enabled = false;
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(5);
        congrats.GetComponent<Text>().text = "Congratulations !";
        word.GetComponent<Text>().text = "You reach the end of the tutorial \n You can now enjoy the full game and increase your vocabulary !";
        next.GetComponent<Text>().text = "Homepage";
    }
}

