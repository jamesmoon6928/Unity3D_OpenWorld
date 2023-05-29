using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    //To prevent error in changing tools
    public static bool isChangeTool = false;

    //Currently equipped tool's ~
    [SerializeField]
    private string currentToolType;
    public static Transform currentTool;
    public static Animator currentToolAnim;

    //Delay when changing tool
    [SerializeField]
    private float changeToolDelayTime;
    [SerializeField]
    private float changetoolEndDelayTime;

    //Types of tools
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    //Dictionary for easier access to a specific tool
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    //Will work like a "switch" whenver switching happens
    [SerializeField]
    private GunInteract gunCtrl;
    [SerializeField]
    private HandInteract handCtrl;
    [SerializeField]
    private AxeCtrl axeCtrl;
    [SerializeField]
    private PickaxeCtrl pickaxeCtrl;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeTool)
        {   //Tool change - Hand
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeToolCoroutine("HAND", "Idle"));
            //Tool change - Gun
            } else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeToolCoroutine("GUN", "SubMachineGun1"));
            //Tool change - Axe
            } else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeToolCoroutine("AXE", "Axe"));
            //Tool change - Pickaxe
            } else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartCoroutine(ChangeToolCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }

    public IEnumerator ChangeToolCoroutine(string type, string name)
    {
        isChangeTool = true;
        currentToolAnim.SetTrigger("Tool_Out");

        yield return new WaitForSeconds(changeToolDelayTime);

        CancelPreToolAction();
        ToolChange(type, name);

        yield return new WaitForSeconds(changetoolEndDelayTime);

        currentToolType = type;
        isChangeTool = false;
    }

    private void CancelPreToolAction()
    {
        switch(currentToolType)
        {
            case "GUN":
                gunCtrl.CancelFineSight();
                gunCtrl.CancelReload();
                GunInteract.isActivated = false;
                break;
            case "HAND":
                HandInteract.isActivated = false;
                break;
            case "AXE":
                AxeCtrl.isActivated = false;
                break;
            case "PICKAXE":
                PickaxeCtrl.isActivated = false;
                break;
        }
    }

    private void ToolChange(string type, string name)
    {
        if(type == "GUN")
        {
            gunCtrl.GunChange(gunDictionary[name]);
        } else if(type == "HAND")
        {
            handCtrl.CloseWeaponChange(handDictionary[name]);
        } else if (type == "AXE")
        {
            axeCtrl.CloseWeaponChange(axeDictionary[name]);
        } else if (type == "PICKAXE")
        {
            pickaxeCtrl.CloseWeaponChange(pickaxeDictionary[name]);
        }
    }
}
