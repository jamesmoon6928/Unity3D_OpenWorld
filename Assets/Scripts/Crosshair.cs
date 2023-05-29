using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private float gunAccuracy;  //according to crosshair type

    //parent object for crosshair deactivation
    [SerializeField]
    private GameObject crosshairHUD;
    [SerializeField]
    private GunInteract gunCtrl;

    public void WalkingAnimation(bool flag)
    {
        ToolManager.currentToolAnim.SetBool("Walk", flag);
        anim.SetBool("Walking", flag);
    }

    public void RunningAnimation(bool flag) {
        ToolManager.currentToolAnim.SetBool("Run", flag);
        anim.SetBool("Running", flag);
    }
    public void JumpingAnimation(bool flag)
    {
        ToolManager.currentToolAnim.SetBool("Run", flag);
        anim.SetBool("Running", flag);
    }

    public void CrouchingAnimation(bool flag)
    {
        anim.SetBool("Crouching", flag);
    }
    public void FineSightAnimation(bool flag)
    {
        anim.SetBool("FineSighted", flag);
    }


    public void FireAnimation()
    {
        if(anim.GetBool("Walking"))
        {
            anim.SetTrigger("Walk_Fire");
        } else if(anim.GetBool("Crouching")) {
            anim.SetTrigger("Crouch_Fire");
        } else
        {
            anim.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (anim.GetBool("Walking"))
        {
            gunAccuracy = 0.06f;
        } else if(anim.GetBool("Crouching"))
        {
            gunAccuracy = 0.015f;
        } else if (gunCtrl.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        } else
        {
            gunAccuracy = 0.03f;
        }
        return gunAccuracy;
    }
}
