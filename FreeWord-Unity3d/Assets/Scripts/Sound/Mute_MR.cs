﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mute_MR : MonoBehaviour
{

    public void MMR ()
    {
        GameObject obj = GameObject.Find("Mute");
        float posX = obj.transform.position.x;

        posX += 110;

        obj.transform.position = new Vector3(posX, obj.transform.position.y, obj.transform.position.z);
    }
}
