using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    Transform curPos;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Destroy(sprite);
        curPos = transform;
    }

    public Transform getTransform(float yPos)
    {
        curPos.position = new Vector3(curPos.position.x, yPos, curPos.position.z);
        return curPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
