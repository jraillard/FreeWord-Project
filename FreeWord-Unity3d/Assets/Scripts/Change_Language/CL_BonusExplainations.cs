using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CL_BonusExplainations : MonoBehaviour
{

    public GameObject title;
    public GameObject back;
    public GameObject next;
    public GameObject bonus1;
    public GameObject bonus2;
    public GameObject infosb1;
    public GameObject infosb2;
    private Data data;

    private string Username;
    private string[] Lines;
    private string[] Line;

    // Use this for initialization
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

    private IEnumerator CL_English()
    {
        title.GetComponent<Text>().text = "If you are stuck, you can use bonus to be able to reach the creation of the word";
        back.GetComponent<Text>().text = "Back";
        bonus1.GetComponent<Text>().text = "Replay";
        bonus2.GetComponent<Text>().text = "Word";
        infosb1.GetComponent<Text>().text = "Replace the letters in their original positions";
        infosb2.GetComponent<Text>().text = "Reveal letters at the good place when the area is empty";
        yield return new WaitForSeconds(6);
        next.GetComponent<Text>().text = "Next";
    }

    private IEnumerator CL_Français()
    {
        title.GetComponent<Text>().text = "Si vous êtes coincé, utilisez les bonus pour pouvoir compléter les mots";
        back.GetComponent<Text>().text = "Retour";
        bonus1.GetComponent<Text>().text = "Replacer";
        bonus2.GetComponent<Text>().text = "Mot";
        infosb1.GetComponent<Text>().text = "Replace les lettres dans leur position originale";
        infosb2.GetComponent<Text>().text = "Revèle plusieurs lettres quand la zone de création est vide";
        yield return new WaitForSeconds(6);
        next.GetComponent<Text>().text = "Suivant";
    }
}
