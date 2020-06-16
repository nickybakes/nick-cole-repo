using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class PlayerLobbyCursor : NetworkBehaviour
{
    private static Color32[] cursorColors = {new Color32(255, 40, 40, 255), new Color32(17, 61, 255, 255), new Color32(255, 246, 9, 255), new Color32(0, 188, 37, 255),
    new Color32(252, 156, 2, 255), new Color32(135, 19, 193, 255), new Color32(255, 0, 238, 255), new Color32(2, 214, 221, 255), new Color32(40,40,40, 255)};

    public Text playerNumberText;
    public Text playerNumberTextShadow;
    public Image cursorImage;
    public Text usernameText;

    private int playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
    }

    public void InitializePlayerCursor(int playerIndex)
    {
        this.playerIndex = playerIndex;
        playerNumberText.text = "P" + (playerIndex + 1);
        playerNumberTextShadow.text = "P" + (playerIndex + 1);
        cursorImage.color = cursorColors[playerIndex];
        usernameText.text = "player " + (playerIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {

        //if (!isLocalPlayer)
        //    return;
        //Debug.Log("s");
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
