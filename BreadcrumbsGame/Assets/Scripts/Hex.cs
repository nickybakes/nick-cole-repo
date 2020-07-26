using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Edge
{
    LeftTop, RightTop, RightMid, RightBot, LeftBot, LeftMid
}

public enum Position
{
    Top, RightTop, RightBot, Bot, LeftBot, LeftTop
}
public class Hex : MonoBehaviour
{


    public GameObject[] edgeButtons;
    public GameObject[] positions;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
