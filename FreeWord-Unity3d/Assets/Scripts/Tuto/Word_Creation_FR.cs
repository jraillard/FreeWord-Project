using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Word_Creation_FR : MonoBehaviour
{

    public GameObject next;

    void Start()
    {
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(8);
        next.GetComponent<Text>().text = "Suivant";
    }
}

