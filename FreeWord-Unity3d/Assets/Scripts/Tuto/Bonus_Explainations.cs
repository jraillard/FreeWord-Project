using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Bonus_Explainations : MonoBehaviour
{
    public GameObject next;

    private Data data;
    private string Username;
    private string[] Lines;
    private string[] Line;

    void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        if (data.LanguageToPlay == "English")
        {
            StartCoroutine(CL_English());
        }
        else
        {
            StartCoroutine(CL_Français());
        }
    }

    IEnumerator CL_English()
    {
        yield return new WaitForSeconds(6);
        next.GetComponent<Text>().text = "Next";
    }
    IEnumerator CL_Français()
    {
        yield return new WaitForSeconds(6);
        next.GetComponent<Text>().text = "Suivant";
    }
}

