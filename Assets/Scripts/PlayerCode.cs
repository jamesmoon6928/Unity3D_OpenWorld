using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerCode : MonoBehaviour
{
    //public InputDevice leftCtrller;
    //public InputDevice rightCtrller;
    //public InputDevice dev_HMD;

    //speed value / By being in serializefield, can be modified in the inspector window
    [SerializeField]
    private float walk;     //speed when walking

    [SerializeField]
    private float crouch_spd;   //speed when crouching

    [SerializeField]
    private float run;      //speed when running
    private float speed;    //switch speed walk <-> run

    [SerializeField]
    private float jumpForce;

    private bool isWalk = false;
    private bool isRun = false;     //boolean to check the character's motion
    private bool isCrouch = false;
    private bool isGround = true;

    //variable that checks movement
    private Vector3 lastPos;

    [SerializeField]
    private float crouchPosY;   //when crouched
    private float defPosY;      //default y pos
    private float checkPosY;    //switch y pose crouch <-> def

    private CapsuleCollider capColl;    //to check if character('s capsule collider) is on the ground

    [SerializeField]
    private float camSensitivity;   //Camera Sensitivity

    [SerializeField]            //Set a limit to camera's rotation like human
    private float camRotationLimit;
    private float curCamRotationX = 0f;

    [SerializeField]        //Necessary components
    private Camera cam;
    private Rigidbody rb;
    private GunInteract gunController;
    private Crosshair crosshair;
    private StatusCtrl statusCtrl;

    // Start is called before the first frame update
    void Start()
    {
        capColl = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();     //Get Rigidbody component
        gunController = FindObjectOfType<GunInteract>();
        crosshair = FindObjectOfType<Crosshair>();
        statusCtrl = FindObjectOfType<StatusCtrl>();

        //initialize camera's y position
        speed = walk;                  //Default speed = walk(ing)
        defPosY = cam.transform.localPosition.y;
        checkPosY = defPosY;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!leftCtrller.isValid)
        //{
        //    InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref leftCtrller);
        //}
        //if (!rightCtrller.isValid)
        //{
        //    InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref rightCtrller);
        //}
        //if (!dev_HMD.isValid)
        //{
        //    InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref dev_HMD);
        //}
        IsGround();
        getJump();
        IfRunning();
        IfCrouch();
        Move();
        MoveCheck();
        CamRotation();
        CharRotation();
    }

    //private void InitializeInputDevice(InputDeviceCharacteristics inputDeviceCharacteristics, ref InputDevice inputDevice)
    //{
    //    List<InputDevice> devices = new List<InputDevice>();
    //    InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, devices);

    //    if (devices.Count > 0)
    //    {
    //        inputDevice = devices[0];
    //    }
    //}

    //Checks if the character is on the ground with the ray casting
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capColl.bounds.extents.y + 0.1f);
        //Don't use -transform.up as raycast always should go down -> V3.down
        //extent - half (size), so extent.y = half of y = half of collider height = half of char height
        //0.1f for margin of error i.e)character is leaned forward a bit, rough ground...
        crosshair.JumpingAnimation(!isGround);
    }

    //When Jump key entered, run Jump() code/function.
    private void getJump()
    {
        if (Input.GetButtonDown("Jump") && isGround && statusCtrl.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    //When player is on the ground, and jump key entered
    private void Jump()
    {
        //When jump key entered while crouching, cancel crouch
        if (isCrouch)
        {
            Crouch();
        }
        statusCtrl.DecreaseStamina(100);
        rb.velocity = transform.up * jumpForce;
    }

    //Makes character run while run-key is entered
    private void IfRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift) && statusCtrl.GetCurrentSP() > 0)
        {
            Run();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || statusCtrl.GetCurrentSP() <= 0)
        {
            RunEnd();
        }
    }

    //Makes character run
    private void Run()
    {
        //When run key input
        if(isCrouch)
        {
            Crouch();
        }

        gunController.CancelFineSight();

        isRun = true;
        crosshair.RunningAnimation(isRun);
        statusCtrl.DecreaseStamina(5);
        speed = run;
    }

    //When run-key input is over, end running
    private void RunEnd()
    {
        isRun = false;
        crosshair.RunningAnimation(isRun);
        speed = walk;
    }

    //When crouch-key entered, make character crouch
    private void IfCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        crosshair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            speed = crouch_spd;
            checkPosY = crouchPosY;
        } else
        {
            speed = walk;
            checkPosY = defPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }

    //Changes the point of view when crouching for naturalness
    IEnumerator CrouchCoroutine()
    {
        float posY = cam.transform.localPosition.y;
        int count = 0;
        while(posY != checkPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, checkPosY, 0.3f);
            cam.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 15)
            {
                break;
            }
            yield return null;
        }
        cam.transform.localPosition = new Vector3(0, checkPosY, 0);
    }

    private void Move()
    {
        //X = L / R
        float moveX = Input.GetAxisRaw("Horizontal");      //1 if กๆ, -1 if ก็ entered. 0 for no input
        //Z = Front / Back
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveX;
        Vector3 moveVertical = transform.forward * moveZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;      //normalize -> get the heading direction
        // Multiply by walkspeed to get the true velocity

        //Make player move by adjusting it's position
        rb.MovePosition(transform.position + velocity * Time.deltaTime);
        //Time.deltaTime to make it move the right amount at each frame

    }

    private void MoveCheck()
    {
        if(!isRun && !isCrouch && isGround)
        {
            if(Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            } else
            {
                isWalk = false;
            }
            crosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
        

    }

    private void CamRotation()          //Camera Rotation - Only ก่, ก้ here
    {
        float xRot = Input.GetAxisRaw("Mouse Y");
        float camRotX = xRot * camSensitivity;
        curCamRotationX -= camRotX;
        curCamRotationX = Mathf.Clamp(curCamRotationX, -camRotationLimit, camRotationLimit);

        cam.transform.localEulerAngles = new Vector3(curCamRotationX, 0f, 0f);
    }

    private void CharRotation()         //Character Rotation
    {
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 charRotY = new Vector3(0f, yRot, 0f) * camSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(charRotY));
    }
}
