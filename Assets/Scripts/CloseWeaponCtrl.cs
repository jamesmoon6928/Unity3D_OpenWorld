using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponCtrl : MonoBehaviour
{
    

    //Current Hand status - holding what kind of object
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    //Action Stauts
    protected bool isAttack = false;
    protected bool isSwing = false;

    //Use Raycast to check the info of object that got hit by character
    protected RaycastHit hitInfo;



    protected void Attack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");      //Set the trigger "attack" in the hand animator to run the animation

        yield return new WaitForSeconds(currentCloseWeapon.atkDelayOn);
        isSwing = true;
        //Attack activated
        StartCoroutine(Hitcoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.atkDelayOff);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.atkDelay - currentCloseWeapon.atkDelayOn - currentCloseWeapon.atkDelayOff);
        isAttack = false;
    }

    //Leave this as abstract. Children classes will finish up this function
    protected abstract IEnumerator Hitcoroutine();

    protected bool CheckObject()
    {
        //Do Raycast from chracter's pos to char's forward and print out hitInfo and set range
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        //Nothing hit
        return false;
    }

    //This function is done, but leaving it as virtual means that this can be editted.
    public virtual void CloseWeaponChange(CloseWeapon closeWpn)
    {
        if (ToolManager.currentTool != null)
        {
            ToolManager.currentTool.gameObject.SetActive(false);
        }
        currentCloseWeapon = closeWpn;
        ToolManager.currentTool = currentCloseWeapon.GetComponent<Transform>();
        ToolManager.currentToolAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
