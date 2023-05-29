using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCtrl : CloseWeaponCtrl
{
    public static bool isActivated = false;



    // Update is called once per frame
    void Update()
    {
        if(isActivated)
        {
            Attack();
        }
    }

    protected override IEnumerator Hitcoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon closeWpn)
    {
        base.CloseWeaponChange(closeWpn);
        isActivated = true;
    }
}
