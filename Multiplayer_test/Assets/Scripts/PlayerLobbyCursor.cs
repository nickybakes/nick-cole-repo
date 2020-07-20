using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerLobbyCursor : NetworkBehaviour
{
    private static Color32[] cursorColors = {new Color32(255, 40, 40, 255), new Color32(17, 61, 255, 255), new Color32(255, 246, 9, 255), new Color32(0, 188, 37, 255),
    new Color32(252, 156, 2, 255), new Color32(135, 19, 193, 255), new Color32(255, 0, 238, 255), new Color32(2, 214, 221, 255), new Color32(40,40,40, 255)};

    private CursorInput cursorInput;

    private Vector2 movementInput;

    private Canvas canvas;

    public Text playerNumberText;
    public Text playerNumberTextShadow;
    public Image cursorImage;
    public Text usernameText;

    public PlayerInput playerInput;

    private int speed = 7;

    [SyncVar] private int playerIndex;
    private bool keyboardControlled;
    private int gamepadDeviceId;
    private Vector2 moveVector;

    // Start is called before the first frame update
    void Start()
    {
        if (!hasAuthority)
        {
            Destroy(gameObject.GetComponent<PlayerInput>());
        }


        cursorInput = new CursorInput();


        canvas = GameObject.FindObjectOfType<Canvas>();
        transform.SetParent(canvas.transform);
        playerNumberText.text = "P" + (playerIndex + 1);
        playerNumberTextShadow.text = "P" + (playerIndex + 1);
        cursorImage.color = cursorColors[playerIndex];
        usernameText.text = "gamepad " + gamepadDeviceId;
    }

    public void InitializePlayerCursor(int playerIndex, bool keyboardControlled, int gamepadDeviceId)
    {
        this.playerIndex = playerIndex;
        Debug.Log(gamepadDeviceId);
        this.keyboardControlled = keyboardControlled;
        this.gamepadDeviceId = gamepadDeviceId;

        //if (keyboardControlled)
        //{
        //    playerInput.defaultControlScheme = cursorInput.KeyboardScheme.name;
        //}
        
        playerNumberText.text = "P" + (playerIndex + 1);
        playerNumberTextShadow.text = "P" + (playerIndex + 1);
        cursorImage.color = cursorColors[playerIndex];
        usernameText.text = "gamepad " + gamepadDeviceId;
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;

        if (!Application.isFocused)
            return;

        float transformX = gameObject.transform.position.x + moveVector.x;
        float transformY = gameObject.transform.position.y + moveVector.y;

        if (transformX > Screen.width - (Screen.width/2))
            transformX = Screen.width - (Screen.width/2);
        else if (transformX < 0 - (Screen.width/2))
            transformX = 0 - (Screen.width/2);

        if (transformY > Screen.height - (Screen.height / 2))
            transformY = Screen.height - (Screen.height / 2);
        else if (transformY < 0 - (Screen.height / 2))
            transformY = 0;

        gameObject.transform.position = new Vector3(transformX, transformY, gameObject.transform.position.z);
    }



    /*    
 *    Old code
 *    if (Gamepad.all[gamepadDeviceId].buttonEast.wasPressedThisFrame)
    {
        Debug.Log(gamepadDeviceId);
    }

    if (Gamepad.all[gamepadDeviceId].dpad.up.isPressed)
    {
        yInput = 1;
        transform.position = new Vector3(transform.position.x, transform.position.y + speed, 0);
    }
    if (Gamepad.all[gamepadDeviceId].dpad.down.isPressed)
    {
        yInput = -1;
        transform.position = new Vector3(transform.position.x, transform.position.y - speed, 0);
    }
    if (Gamepad.all[gamepadDeviceId].dpad.left.isPressed)
    {
        xInput = -1;
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, 0);
    }
    if (Gamepad.all[gamepadDeviceId].dpad.right.isPressed)
    {
        xInput = 1;
        transform.position = new Vector3(transform.position.x + speed, transform.position.y, 0);
    }

    //RectTransform objectRectTransform = canvas.GetComponent<RectTransform>();
    //yPos = yPos + (yInput * speed * canvas.transform.localScale.y);
    //xPos = xPos + (xInput * speed * canvas.transform.localScale.x);
    //yPos = Mathf.Clamp(yPos, 0, objectRectTransform.rect.height * canvas.transform.localScale.y);
    //xPos = Mathf.Clamp(xPos, 0, objectRectTransform.rect.width * canvas.transform.localScale.x);

    //gameObject.transform.position = new Vector2(xPos, yPos);

    //transform.position += (Vector3)movementInput * 5;
    */
}
