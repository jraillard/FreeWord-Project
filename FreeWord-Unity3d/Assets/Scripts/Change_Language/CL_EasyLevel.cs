using UnityEngine;
using UnityEngine.UI;

public class CL_EasyLevel : MonoBehaviour
{

    public GameObject bonus1;
    public GameObject bonus2;
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
            CL_English();
        }
        else
        {
            CL_Français();
        }
    }

    private void CL_English()
    {
        bonus1.GetComponent<Text>().text = "Word";
        bonus2.GetComponent<Text>().text = "Replay";
    }

    private void CL_Français()
    {
        bonus1.GetComponent<Text>().text = "Mot";
        bonus2.GetComponent<Text>().text = "Replace";
    }
}
