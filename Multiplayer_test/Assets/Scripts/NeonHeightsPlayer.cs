using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class NeonHeightsPlayer : NetworkBehaviour
{

    private PlayerInput playerInput;
    [SyncVar] private bool initialized = false;
    private InputDevice device;

    [SyncVar] private int playerIndex;
    [SyncVar] private bool keyboardControlled;
    private int gamepadDeviceId;
    private Vector2 moveVector;
    public bool canMove;

    private NeonHeightsLobbyClient2 clientHandler;

    void Start()
    {
        if (!hasAuthority)
        {
            Destroy(gameObject.GetComponent<PlayerInput>());
        }
        else
        {
            canMove = false;
            playerInput = gameObject.GetComponent<PlayerInput>();
            print("PlayerInput: " + playerInput);
            playerInput.neverAutoSwitchControlSchemes = true;
            clientHandler = GameObject.FindObjectOfType<NeonHeightsDataHandler>().GetLocalClient();

            if (initialized)
            {
                clientHandler.AddPlayer(this);
            }
        }
    }

    public InputDevice GetDevice()
    {
        return device;
    }

    public void PairToDevice(InputDevice device)
    {
        if (!hasAuthority)
            return;

        print("PairToDevice called" + device);

        this.device = device;

        playerInput.user.UnpairDevices();

        print("Input user: " + InputUser.PerformPairingWithDevice(device, playerInput.user, InputUserPairingOptions.None));
        print("Paired devices: " + playerInput.user.pairedDevices.Count);
        print("Lost Devices: " + playerInput.user.lostDevices.Count);
        print("Switch Control Scheme: " + playerInput.SwitchCurrentControlScheme(new InputDevice[] { device }));

    }

    public int GetPlayerNum()
    {
        return playerIndex + 1;
    }

    public void InitializePlayer(int playerIndex)
    {
        initialized = true;

        this.playerIndex = playerIndex - 1;
    }

    public void OnMove(InputValue value)
    {
        if (!hasAuthority)
            return;

        if (!Application.isFocused)
            return;

        print("move called");

        moveVector = value.Get<Vector2>();
    }

    public void Move()
    {

        float transformX = gameObject.transform.position.x + moveVector.x;
        float transformY = gameObject.transform.position.y + moveVector.y;

        if (transformX > Screen.width - (Screen.width / 2))
            transformX = Screen.width - (Screen.width / 2);
        else if (transformX < 0 - (Screen.width / 2))
            transformX = 0 - (Screen.width / 2);

        if (transformY > Screen.height - (Screen.height / 2))
            transformY = Screen.height - (Screen.height / 2);
        else if (transformY < 0 - (Screen.height / 2))
            transformY = 0;

        gameObject.transform.position = new Vector3(transformX, transformY, gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;

        if (!Application.isFocused)
            return;

        if (canMove)
        {
            Move();
        }
    }
}
