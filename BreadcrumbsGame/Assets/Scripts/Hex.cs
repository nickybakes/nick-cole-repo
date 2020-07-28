using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public enum Edge
{
    LeftTop, RightTop, RightMid, RightBot, LeftBot, LeftMid, None
}

public enum Position
{
    Top, RightTop, RightBot, Bot, LeftBot, LeftTop
}
public class Hex : MonoBehaviour
{


    public GameObject[] edgeButtons;
    public GameObject[] positions;

    private Edge badEdge;


    // Start is called before the first frame update
    void Start()
    {
        badEdge = (Edge) UnityEngine.Random.Range(0, 7);


    }

    // Update is called once per frame
    void Update()
    {
        if (badEdge != Edge.None)
        {
            //edgeButtons[(int)badEdge].SetActive(false);
        }
    }
}
