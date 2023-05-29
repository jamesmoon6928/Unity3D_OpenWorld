using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeCtrl : CloseWeaponCtrl
{
    public static bool isActivated = true;

    private void Start()
    {
        ToolManager.currentTool = currentCloseWeapon.GetComponent<Transform>();
        ToolManager.currentToolAnim = currentCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            Attack();
        }
    }

    protected override IEnumerator Hitcoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if(hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                } else if (hitInfo.transform.tag == "NPC")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<Pig>().Damage(currentCloseWeapon.damage, transform.position);
                }
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
