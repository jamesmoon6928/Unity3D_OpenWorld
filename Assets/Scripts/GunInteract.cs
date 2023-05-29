using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunInteract : MonoBehaviour
{
    public static bool isActivated = false;

    //Currently equiped gun
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private bool isReload = false;
    [HideInInspector]
    public bool isFSMode = false; //Fine sight mode

    //Gun's original position to come back after fine sight
    [SerializeField]
    private Vector3 originPos;

    private AudioSource audioSource;

    private RaycastHit hitInfo;

    [SerializeField]
    private Camera cam;
    private Crosshair crosshair;

    [SerializeField]
    private GameObject hit_effect_prefab;

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        crosshair = FindObjectOfType<Crosshair>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            GunFireRateCalc();
            IfTriggered();
            IfReload();
            IfFineSight();
        }
    }

    //Recalculate fire rate speed
    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;      //deltaTime = 1/60 sec, 1 sec = 60 frame, 1/60 sec X 60 = 1
        }
    }

    //When Fire key entered, call StartFire() function
    private void IfTriggered()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            StartFire();
        }
    }

    //Check the current status, then call Fire() if not reloading and have some bullets
    private void StartFire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Fire();
            }
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    //Function for each shot/fire
    private void Fire()
    {
        crosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;  //Re-calc fire rate
        PlaySound(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Hit();
        StopAllCoroutines();
        StartCoroutine(RecActionCoroutine());

        //Debug.Log("Gun Fired");
    }

    //Check the object with raycast, and when the object got hit, create a hit effect on the object
    private void Hit()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward + 
            new Vector3(Random.Range(-crosshair.GetAccuracy() - currentGun.accuracy, crosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-crosshair.GetAccuracy() - currentGun.accuracy, crosshair.GetAccuracy() + currentGun.accuracy), 0),
            out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //Destroy instantiated variable after 2secs.
            Destroy(clone, 2f);
            if (hitInfo.transform.tag == "NPC")
            {
                hitInfo.transform.GetComponent<Pig>().Damage(currentGun.damage, transform.position);
            }
        }
    }

    //When reload key was entered, check bullet count and then execute if user has some
    private void IfReload()
    {
        if(Input.GetKeyDown(KeyCode.R) &&!isReload && (currentGun.currentBulletCount < currentGun.reloadBulletCount))
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    //Reload and calc bullet count
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.totalBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.totalBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.totalBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.totalBulletCount -= currentGun.reloadBulletCount;
            } else
            {
                currentGun.currentBulletCount = currentGun.totalBulletCount;
                currentGun.totalBulletCount = 0;
            }
            isReload = false;
        } else
        {
            Debug.Log("Run out of bullet");
        }
    }

    //When fine sight key entered, shouldn't be reloading
    private void IfFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    //Cancel fine sight mode
    public void CancelFineSight()
    {
        if(isFSMode)
        {
            FineSight();
        }
    }

    //Activate/Deactivate Fine sight coroutine depend on current state
    private void FineSight()
    {
        isFSMode = !isFSMode;
        currentGun.anim.SetBool("FSMode", isFSMode);
        crosshair.FineSightAnimation(isFSMode);
        if(isFSMode)
        {
            StopAllCoroutines();
            StartCoroutine(FSModeActivateCoroutine());
        } 
        else
        {
            StopAllCoroutines();
            StartCoroutine(FSModeDeactivateCoroutine());
        }
    }
    
    //Activate fine sight mode
    IEnumerator FSModeActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginpos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginpos, 0.2f);
            yield return null;
        }
    }

    //Deactivate fine sight mode
    IEnumerator FSModeDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    //Recoil movement function
    IEnumerator RecActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.recForce, originPos.y, originPos.z);
        Vector3 recoilFSBack = new Vector3(currentGun.recFineSightForce, currentGun.fineSightOriginpos.y, currentGun.fineSightOriginpos.z);
        if (!isFSMode)
        {
            currentGun.transform.localPosition = originPos;
            //Recoil start
            while(currentGun.transform.localPosition.x <= currentGun.recForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //Return to originpos after recoil
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else    //Fine Sight Mode Recoil
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginpos;
            //Recoil start
            while (currentGun.transform.localPosition.x <= currentGun.recFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilFSBack, 0.4f);
                yield return null;
            }

            //Return to FS-originpos after recoil
            while (currentGun.transform.localPosition != currentGun.fineSightOriginpos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginpos, 0.1f);
                yield return null;
            }
        }
    }

    //Audioclip play function for sound whenever fired
    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public bool GetFineSightMode()
    {
        return isFSMode;
    }

    public void GunChange(Gun gun)
    {
        if(ToolManager.currentTool != null)
        {
            ToolManager.currentTool.gameObject.SetActive(false);
        }
        currentGun = gun;
        ToolManager.currentTool = currentGun.GetComponent<Transform>();
        ToolManager.currentToolAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivated = true;
    }
}
