﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildScale : MonoBehaviour
{
    public Vector3 defaultScale = Vector3.zero;


    void Update()
    {
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;

        transform.localScale = new Vector3(
                localScale.x / lossScale.x * defaultScale.x,
                localScale.y / lossScale.y * defaultScale.y,
                localScale.z / lossScale.z * defaultScale.z
        );
    }
}
