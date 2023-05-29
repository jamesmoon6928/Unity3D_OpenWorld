using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName;

    //Possible type of tools
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range;         //Hand's interaction range
    public int damage;          //Attack damage
    public float workSpeed;     //Work speed
    public float atkDelay;      //Delay for each attack
    public float atkDelayOn;    //Activate attack delay - prevent delay-less continuous attack
    public float atkDelayOff;   //Deactivate attack delay


    public Animator anim;       //Character's hands animation
    
}
