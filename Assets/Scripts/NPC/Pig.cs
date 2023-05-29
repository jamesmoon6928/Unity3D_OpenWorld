using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;
    [SerializeField] private int hp;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;       //speed var to switch between speeds

    private Vector3 direction;  //pig's movement direction

    //bool value for motion check
    private bool isWalking;
    private bool isAction;
    private bool isRunning;
    private bool isDead;

    [SerializeField] private float walkTime;
    [SerializeField] private float waitTime;     //delay after each action
    [SerializeField] private float runTime;

    private float currentTime;

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider boxCol;
    private AudioSource audio;

    [SerializeField] private AudioClip[] aud_pig_sound;
    [SerializeField] private AudioClip aud_pig_hurt;
    [SerializeField] private AudioClip aud_pig_Dead;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            rb.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }

    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 rot = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rb.MoveRotation(Quaternion.Euler(rot));
        }
    }

    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                ResetAction();
            }
        }
    }

    private void ResetAction()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    private void RandomAction()
    {
        int random = Random.Range(0, 4);

        if(random == 0)
        {
            Wait();
        } else if(random == 1)
        {
            Eat();
        } else if (random == 2)
        {
            Peek();
        } else if (random == 3)
        {
            IfWalk();
        }
    }

    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("Idle");
    }

    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Eat");
    }

    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("Peek");
    }

    private void IfWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("Walk");
    }

    private void Run(Vector3 targetPos)
    {
        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }

    public void Damage(int dmg, Vector3 targetPos)
    {
        if(!isDead)
        {
            hp -= dmg;

            if(hp <= 0)
            {
                Dead();
                return;
            }
            PlaySE(aud_pig_hurt);
            anim.SetTrigger("Hurt");
            Run(targetPos);
        }
    }

    private void Dead()
    {
        PlaySE(aud_pig_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }

    private void RandomSound()
    {
        int random = Random.Range(0, 3);
        PlaySE(aud_pig_sound[random]);
    }

    private void PlaySE(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();
    }
}
