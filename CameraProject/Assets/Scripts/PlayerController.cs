using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public bool isLocalPlayer = true, canGetFrame = false;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    private float AIhAxis = 0, AIvAxis = 0;
    public int changeDirChance = 20;

    public const int FRAME_HEIGHT = 1080, FRAME_WIDTH = 1920;
    private Vector3 startPos, curPos, newPos;
    public GameObject renderCamera;
    private Texture2D frameTex;
    private RenderTexture frameRT;
    private byte[] framebuffer;
    private bool readFrame;
    private Rect rectReadPicture = new Rect(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
    private PythonRequester pythonRequester;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        pythonRequester = new PythonRequester(this);
        pythonRequester.Start();
        startPos = Camera.main.transform.position;
        frameTex = new Texture2D(FRAME_WIDTH, FRAME_HEIGHT);
        frameRT = new RenderTexture(FRAME_WIDTH, FRAME_HEIGHT, 0);
        readFrame = true;

        if (isLocalPlayer)
        {
            characterController = GetComponent<CharacterController>();

            // Lock cursors

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Destroy(playerCamera);
        }
    }

    private void getFramePixels()
    {
        Camera cam = renderCamera.GetComponent(typeof(Camera)) as Camera;
        if (cam == null)
            return;

        cam.targetTexture = frameRT;
        cam.Render();
        newPos = cam.transform.position - startPos;


        if (readFrame)
        {
            RenderTexture.active = frameRT;
            frameTex.ReadPixels(rectReadPicture, 0, 0);
            readFrame = false;
        } else
        {
            framebuffer = ImageConversion.EncodeToPNG(frameTex);
            curPos = newPos;
            readFrame = true;
            canGetFrame = true;
        }



    }

    public bool CanGetFrame()
    {
        return canGetFrame;
    }

    public byte[] getFrame()
    {
        return framebuffer;
        canGetFrame = false;
    }


    void Update()
    {
        getFramePixels();
        float randDecision = Random.Range(0.0f, 1.0f) * 100;
        if (randDecision < changeDirChance)
        {
            AIvAxis = (float)Random.Range(-1.0f, 1.0f);
            AIhAxis = (float)Random.Range(-1.0f, 1.0f);
        }
        
        if (isLocalPlayer)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * AIvAxis/*Input.GetAxis("Vertical")*/ : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * AIhAxis/*Input.GetAxis("Horizontal")*/ : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
    }
}
