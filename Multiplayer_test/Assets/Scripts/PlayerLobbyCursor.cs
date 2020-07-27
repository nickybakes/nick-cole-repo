using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

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

    private PlayerInput playerInput;
    [SyncVar] private bool initialized = false;
    private InputDevice device;

    private int speed = 7;

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
        transform.SetParent(canvas.transform);
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
