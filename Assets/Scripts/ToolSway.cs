using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSway : MonoBehaviour
{
    //Tool's original postiion when drawn
    private Vector3 originPos;

    //Tool's current position
    private Vector3 curPos;

    //Set limit of swaying
    [SerializeField]
    private Vector3 limitPos;

    //Set limit of swaying for fine sight mode
    [SerializeField]
    private Vector3 fineSightLimitPos;

    //smooth level of swaying
    [SerializeField]
    private Vector3 smoothSway;

    [SerializeField]
    private GunInteract gunCtrl;

    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        IfSway();
    }

    private void IfSway()
    {
        if (Input.GetAxisRaw("Mouse  X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    private void Swaying()
    {
        float moveX = Input.GetAxisRaw("Mouse X");
        float moveY = Input.GetAxisRaw("Mouse Y");

        //Clamp func to set a limit to the screen, Lerp func to make the screen move slowly, naturally
        if(!gunCtrl.isFSMode)
        {
            curPos.Set(Mathf.Clamp(Mathf.Lerp(curPos.x, -moveX, smoothSway.x), -limitPos.x, limitPos.x),
                   Mathf.Clamp(Mathf.Lerp(curPos.y, -moveY, smoothSway.y), -limitPos.y, limitPos.y),
                   originPos.z);
        } else
        {
            curPos.Set(Mathf.Clamp(Mathf.Lerp(curPos.x, -moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                   Mathf.Clamp(Mathf.Lerp(curPos.y, -moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                   originPos.z);
        }
        
        transform.localPosition = curPos;
    }

    private void BackToOriginPos()
    {
        curPos = Vector3.Lerp(curPos, originPos, smoothSway.x);
        transform.localPosition = curPos;
    }
}
