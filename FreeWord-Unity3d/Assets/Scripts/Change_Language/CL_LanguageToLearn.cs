﻿using UnityEngine;
using UnityEngine.UI;

public class CL_LanguageToLearn : MonoBehaviour
{
    public GameObject back;
    private Data data;
    private GameObject title;

    private string Username;
    private string[] Lines;
    private string[] Line;

    // Use this for initialization
    void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        title = GameObject.Find("Title");
        if (data.LanguageToPlay == "English")
        {
            CL_English();
        }
        else
        {
            CL_Français();
        }
    }

    private void CL_English()
    {
        back.GetComponent<Text>().text = "Back";
        title.GetComponent<Text>().text = " Which language do you want to pratice ? ";
    }

    private void CL_Français()
    {
        back.GetComponent<Text>().text = "Retour";
        title.GetComponent<Text>().text = " Dans quelle langue voulez-vous vous exercer ? ";
    }
}
