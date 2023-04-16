using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBar;
    public Slider accelerationBar;
    private float accelerationHP;
    private float currentHP; //This value exists mostly for readability
    public float accWaitTime; //Sets how long it takes for acceleration to start. In seconds
    public float accSpeed; //How many seconds it takes decay to complete for a full bar
    private float accTime; 
    
    void Start()
    {
        accSpeed = 2f;
        accWaitTime = 1f;
        setHealthBar();
        setAccelerationBar();
        accelerationHP = healthBar.value;
        accTime = accWaitTime; //Mutating float for tracking time;
    }

    void Update()
    {
        currentHP = setHealthBar(); //applies damage

        //Code that implements Health Acceleration
        if (currentHP < accelerationHP) 
        {
            //Health was lost, show acceleration
            if (accTime > 0f) //Waits "accWaitTime" seconds to start lowering the acceleration bar
            {
                accTime -= 1f * Time.deltaTime; 
            }
            else //After "accWaitTime" seconds have passed, start decay
            {
                accelerationBar.value -= (healthBar.maxValue * Time.deltaTime) / accSpeed;
                if (accelerationBar.value <= currentHP) 
                {
                    accelerationHP = currentHP;
                }
            }
        }
        else
        {
            //Health stays the same or is gained, set Acceleration bar to the HP bar
            setAccelerationBar();
            accTime = accWaitTime;
            accelerationHP = currentHP;
        }
    }

    public void AlertDamage()
    {
        //If damaged again, wait "accWaitTime" seconds again
        accTime = accWaitTime;
    }
    
    private float setHealthBar()
    {
        //Sets the health bar, as well as returning its current value;
        float tempHP = StatusEvents.player.getHealth();
        healthBar.maxValue = StatusEvents.player.getMaxHealth();
        healthBar.value = tempHP;
        return tempHP;
    }

    private void setAccelerationBar()
    {
        //Sets the acceleration bar
        accelerationBar.maxValue = healthBar.maxValue;
        accelerationBar.value = healthBar.value;
    }
}
