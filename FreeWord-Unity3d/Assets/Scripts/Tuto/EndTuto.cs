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
        yield return new WaitForSeconds(5);
        congrats.GetComponent<Text>().text = "Congratulations !";
        word.GetComponent<Text>().text = "You reach the end of the tutorial \n You can now enjoy the full game and increase your vocabulary !";
        next.GetComponent<Text>().text = "Homepage";
    }

    IEnumerator CL_Français()
    {
        yield return new WaitForSeconds(5);
        congrats.GetComponent<Text>().text = "Félicitations !";
        word.GetComponent<Text>().text = "Vous avez fini le tutoriel \n Vous pouvez désormais profiter de la totalité du jeu et améliorer votre vocabulaire !";
        next.GetComponent<Text>().text = "Accueil";
    }

}