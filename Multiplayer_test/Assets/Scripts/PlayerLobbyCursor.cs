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

    public Text playerNumberText;
    public Text playerNumberTextShadow;
    public Image cursorImage;
    public Text usernameText;

    public PlayerInput playerInput;

    [SyncVar]
    private int playerIndex;
    private bool keyboardControlled;
    private int gamepadDeviceId;
    private Keyboard keyboard;
    private Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        cursorInput = new CursorInput();

        transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
        playerNumberText.text = "P" + (playerIndex + 1);
        playerNumberTextShadow.text = "P" + (playerIndex + 1);
        cursorImage.color = cursorColors[playerIndex];
        usernameText.text = "player " + (playerIndex + 1);
    }

    public void InitializePlayerCursor(int playerIndex, Keyboard keyboard, Gamepad gamepad)
    {
        this.playerIndex = playerIndex;
        Debug.Log(gamepadDeviceId);
        this.keyboard = keyboard;
        this.gamepad = gamepad;

        //if (keyboardControlled)
        //{
        //    playerInput.defaultControlScheme = cursorInput.KeyboardScheme.name;
        //}
        
        playerNumberText.text = "P" + (playerIndex + 1);
        playerNumberTextShadow.text = "P" + (playerIndex + 1);
        cursorImage.color = cursorColors[playerIndex];
        usernameText.text = "player " + (playerIndex + 1);
    }

    private void OnMove(InputValue value)
    {
        //cursorInput.bindingMask = InputBinding.MaskByGroup("Gamepad");
        if (!hasAuthority)
            return;

        //if (keyboard != null && Keyboard.current == keyboard)
        //{
        //    Debug.Log("Keyboard move");
        //}
        //else if(Gamepad.current == gamepad)
        //{
        //    Debug.Log(Keyboard.current.deviceId);
        //    movementInput = value.Get<Vector2>();
        //}
        movementInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;

        transform.position += (Vector3)movementInput*5;

        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y + 5, 0);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y - 5, 0);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.position = new Vector3(transform.position.x - 5, transform.position.y, 0);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.position = new Vector3(transform.position.x + 5, transform.position.y, 0);
        //}
    }

    public void UpdateInput()
    {

    }
}
