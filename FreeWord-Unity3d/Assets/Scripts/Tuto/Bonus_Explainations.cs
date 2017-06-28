using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Bonus_Explainations : MonoBehaviour
{
    public GameObject infos;
    public GameObject next;

    void Start()
    {
        StartCoroutine(Time());
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(6);
        next.GetComponent<Text>().text = "Next";
    }
}

