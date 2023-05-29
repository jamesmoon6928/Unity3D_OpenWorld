using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;      //type of gun
    public float range;
    public float accuracy;
    public float fireRate;      //continuous firing speed
    public float reloadTime;    //reloading time

    public int damage;
    public int reloadBulletCount;   //# of bullet to reload(MAX - current bullet)
    public int currentBulletCount;  //# of bullet in the magazine
    public int maxBulletCount;      //possilbe max # of bullet in a magazine
    public int totalBulletCount;    //total # of bullet owned

    public float recForce;          //recoil force
    public float recFineSightForce; //recoil force when fine sighted shooting

    public Vector3 fineSightOriginpos;  //gun's position when fine sighting

    public Animator anim;

    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
