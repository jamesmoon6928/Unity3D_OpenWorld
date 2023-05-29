using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusCtrl : MonoBehaviour
{
    //HP
    [SerializeField]
    private int hp;
    private int currentHp;

    //Stamina
    [SerializeField]
    private int sp;
    private int currentSp;

    //Stamina Regen
    [SerializeField]
    private int spIncreaseSpeed;

    //Stamina Regen Delay
    [SerializeField]
    private int spRegenTime;
    private int currentSpRegenTime;

    //Check if action taken that uses stamina
    private bool spUsed;

    //Defense Point
    [SerializeField]
    private int dp;
    private int currentDp;

    //Hungriness
    [SerializeField]
    private int hunger;
    private int currentHunger;

    //Hunger Decrease
    [SerializeField]
    private int hungerDecreaseTime;
    private int currentHungerDecreaseTime;

    //Images needed
    [SerializeField]
    private Image[] images_Gauge;

    //Set number for each status
    private const int HP = 0, DP = 1, SP = 2, HUNGER = 3;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHunger = hunger;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        SPRegenTime();
        SPRegen();
        GaugeUpdate();
    }

    private void SPRegenTime()
    {
        if (spUsed)
        {
            if (currentSpRegenTime < spRegenTime)
            {
                currentSpRegenTime++;
            } else
            {
                spUsed = false;
            }
        }
    }

    private void SPRegen()
    {
        if(!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry()
    {
        if (currentHunger > 0)
        {
            if (currentHungerDecreaseTime <= hungerDecreaseTime)
            {
                currentHungerDecreaseTime++;
            } else
            {
                currentHunger--;
                currentHungerDecreaseTime = 0;
            }
        } else
        {
            Debug.Log("Character's hunger level has reached 0");
        }
    }

    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float) currentHp / hp;
        images_Gauge[DP].fillAmount = (float) currentDp / dp;
        images_Gauge[SP].fillAmount = (float) currentSp / sp;
        images_Gauge[HUNGER].fillAmount = (float) currentHunger / hunger;
    }
    
    public void IncreaseHP(int count)
    {
        if (currentHp + count < hp)
        {
            currentHp += count;
        } else
        {
            currentHp = hp;
        }
    }

    public void DecreaseHP(int count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(count);
            return;
        }
        currentHp -= count;
        if (currentHp <= 0)
        {
            Debug.Log("Character's HP has reached 0");
        }
    }

    public void IncreaseDP(int count)
    {
        if (currentDp + count < dp)
        {
            currentDp += count;
        }
        else
        {
            currentDp = dp;
        }
    }

    public void DecreaseDP(int count)
    {
        currentDp -= count;
    }

    public void IncreaseHunger(int count)
    {
        if (currentHunger + count < hunger)
        {
            currentHunger += count;
        }
        else
        {
            currentHunger = hunger;
        }
    }

    public void DecreaseHunger(int count)
    {
        if (currentHunger - count < 0)
        {
            currentHunger = 0;
        }
        else
        {
            currentHunger -= count;
        }
    }

    public void DecreaseStamina(int count)
    {
        spUsed = true;
        currentSpRegenTime = 0;

        if(currentSp - count > 0)
        {
            currentSp -= count;
        } else
        {
            currentSp = 0;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}
