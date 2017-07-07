using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Internet_Reachability : MonoBehaviour
{
    public GameObject text;

    void Start()
    {
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(3);
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            GameObject.Find("Main Camera").GetComponent<GoToIntroduction>().Load();
        }
        else
        {
            StartCoroutine(Check());
            text.GetComponent<Text>().text = "Please, check your connection";
        }
    }
}
