using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerLobbyCursor : NetworkBehaviour
{
    private Color32[] cursorColors = {new Color32(255, 40, 40, 255), new Color32(17, 61, 255, 255), new Color32(255, 246, 9, 255), new Color32(0, 188, 37, 255),
    new Color32(252, 156, 2, 255), new Color32(135, 19, 193, 255), new Color32(255, 0, 238, 255), new Color32(2, 214, 221, 255), new Color32(40,40,40, 255)};

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 5, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 5, transform.position.y, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 5, transform.position.y, 0);
        }
    }
}
