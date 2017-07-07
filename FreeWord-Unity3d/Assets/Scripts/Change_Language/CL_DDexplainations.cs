using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CL_DDexplainations : MonoBehaviour
{

    public GameObject title;
    public GameObject next;
    private Data data;

    private string Username;
    private string[] Lines;
    private string[] Line;

    // Use this for initialization
    void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        if(data.LanguageToPlay == "English")
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
        title.GetComponent<Text>().text = "You can move cards in the empty area with the Drag&Drop action";
        yield return new WaitForSeconds(4);
        next.GetComponent<Text>().text = "Next";
    }

    private IEnumerator CL_Français()
    {
        title.GetComponent<Text>().text = "Vous pouvez bouger les cartes en restant appuyer dessus";
        yield return new WaitForSeconds(4);
        next.GetComponent<Text>().text = "Suivant";
    }
}
