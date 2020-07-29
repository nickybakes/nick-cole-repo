using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;
using System.Net;

public class PlayerLobbyCursor : NetworkBehaviour
{
    private const int WIDTH = 1920;
    private const int HEIGHT = 1080;

    private static Color32[] cursorColors = {new Color32(255, 40, 40, 255), new Color32(17, 61, 255, 255), new Color32(255, 246, 9, 255), new Color32(0, 188, 37, 255),
    new Color32(252, 156, 2, 255), new Color32(135, 19, 193, 255), new Color32(255, 0, 238, 255), new Color32(2, 214, 221, 255), new Color32(40,40,40, 255)};

    private CursorInput cursorInput;

    private Vector2 movementInput;

    private Canvas canvas;
    private RectTransform canvasRectTransform;

    public Text playerNumberText;
    public Text playerNumberTextShadow;
    public Image cursorImage;
    public Text usernameText;

    private PlayerInput playerInput;
    [SyncVar] private bool initialized = false;
    private InputDevice device;

    private int speed = 20;

    [SyncVar] private int playerIndex;
    [SyncVar] private bool keyboardControlled;
    private int gamepadDeviceId;
    private Vector2 moveVector;

    private NeonHeightsLobbyClient2 clientHandler;

    // Start is called before the first frame update
    void Start()
    {
        if (!hasAuthority)
        {
            Destroy(gameObject.GetComponent<PlayerInput>());
        }
        else
        {
            playerInput = gameObject.GetComponent<PlayerInput>();
            print("PlayerInput: " + playerInput);
            playerInput.neverAutoSwitchControlSchemes = true;
            clientHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>().GetLocalClient();

            if (keyboardControlled)
            {
                device = Keyboard.current;
            }
            else
            {
                device = Gamepad.current;
            }

            if (initialized)
            {
                clientHandler.AddCursor(this);
            }
        }

        cursorInput = new CursorInput();
        canvas = GameObject.FindObjectOfType<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        transform.SetParent(canvas.transform);
        transform.position = new Vector2(canvas.pixelRect.width/ 2, canvas.pixelRect.height/ 2);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        playerNumberText.text = "P" + (playerIndex + 1);
        playerNumberTextShadow.text = "P" + (playerIndex + 1);
        cursorImage.color = cursorColors[playerIndex];
        usernameText.text = "gamepad " + gamepadDeviceId;
    }

    public InputDevice GetDevice()
    {
        return device;
    }

    public void PairToDevice()
    {
        if (!hasAuthority)
            return;
        InputUser curUser = InputUser.PerformPairingWithDevice(device, playerInput.user, InputUserPairingOptions.None);
        bool debugSwitchControlScheme = playerInput.SwitchCurrentControlScheme(new InputDevice[] { device });
        
    }

    public int GetPlayerNum()
    {
        return playerIndex+1;
    }

    public void InitializePlayerCursor(int playerIndex, bool keyboardControlled)
    {
        print("called");
        initialized = true;

        this.playerIndex = playerIndex-1;
        this.keyboardControlled = keyboardControlled;

        print("keyboardControlled: " + keyboardControlled);

        playerNumberText.text = "P" + (playerIndex + 1);
        playerNumberTextShadow.text = "P" + (playerIndex + 1);
        cursorImage.color = cursorColors[playerIndex];
        usernameText.text = "gamepad " + gamepadDeviceId;
    }

    public void OnMove(InputValue value)
    {
        if (!hasAuthority)
            return;

        if (!Application.isFocused)
            return;

        moveVector = value.Get<Vector2>();
    }

    public void OnSelect(InputValue value)
    {
        if (!hasAuthority)
            return;

        if (!Application.isFocused)
            return;

        print("select called");

        clientHandler.PrepareToStartGame();
    }

    public void OnLeave(InputValue value)
    {
        if (!hasAuthority)
            return;

        if (!Application.isFocused)
            return;
        clientHandler.PlayerLeave(this.GetPlayerNum(), keyboardControlled);
    }


    // Update is called once per frame
    void Update()
    {

        if (!hasAuthority)
            return;

        if (!Application.isFocused)
            return;

        //screenPosX += moveVector.x * movementSpeed;
        //screenPosY += moveVector.y * movementSpeed;

        //screenPosX = Math.Min(.5f, Math.Max(-.5f, screenPosX));
        //screenPosY = Math.Min(.5f, Math.Max(-.5f, screenPosY));

        //float canvasWidth = canvas.transform.localScale.x * WIDTH;
        //float canvasHeight = canvas.transform.localScale.y * HEIGHT;

        //float transformX = Math.Min(canvasWidth / 2, Math.Max(canvasWidth / -2, canvasWidth * screenPosX));
        //float transformY = Math.Min(canvasHeight / 2, Math.Max(canvasHeight / -2, canvasHeight * screenPosY));

        //gameObject.transform.localScale = new Vector3(1, 1, 1);
        //gameObject.transform.position = new Vector3(transformX, transformY, gameObject.transform.position.z);



        //if (transformX > Screen.width - (Screen.width / 2))
        //    transformX = Screen.width - (Screen.width / 2);
        //else if (transformX < 0 - (Screen.width / 2))
        //    transformX = 0 - (Screen.width / 2);

        //if (transformY > Screen.height - (Screen.height / 2))
        //    transformY = Screen.height - (Screen.height / 2);
        //else if (transformY < 0 - (Screen.height / 2))
        //    transformY = 0;

        float transformX = gameObject.transform.position.x + (moveVector.x * speed * canvas.transform.localScale.x);
        float transformY = gameObject.transform.position.y + (moveVector.y * speed * canvas.transform.localScale.y);
        transformX = Mathf.Clamp(transformX, 0, canvasRectTransform.rect.width * canvas.transform.localScale.x);
        transformY = Mathf.Clamp(transformY, 0, canvasRectTransform.rect.height * canvas.transform.localScale.y);

        gameObject.transform.position = new Vector2(transformX, transformY);

        //gameObject.transform.position = new Vector3(Math.Min(WIDTH/2, Math.Max(-WIDTH/2, transformX)),
        //    Math.Min(1080, Math.Max(0, transformY)), gameObject.transform.position.z);
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
