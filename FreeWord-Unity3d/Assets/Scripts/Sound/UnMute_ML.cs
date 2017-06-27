using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnMute_ML : MonoBehaviour
{

    public void UMML ()
    {
        GameObject obj = GameObject.Find("Unmute");
        float posX = obj.transform.position.x;

        posX -= 110;

        obj.transform.position = new Vector3(posX, obj.transform.position.y, obj.transform.position.z);
    }
}

