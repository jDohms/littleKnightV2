using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public float health = 100;
    public float maxHealth = 100;
    public float stamina = 100;
    public float maxStamina = 100;
    public float staminaRegen = 5;
    public float healthRegen = 1;
    public bool dead;
    public bool iframes;
    public float iframesMaxTime = 3;
    float iframesTimer;
    public bool blocking;


    Animator anim;

    void Start ()
    {
        health = maxHealth;
        stamina = maxStamina;
        anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        HandleRegen();
        HandleIFrames();

        if (health <= 0)
        {
            anim.SetBool("Dead", true);
            dead = true;
            
        }
	}

    void HandleIFrames()
    {
        if (iframes)
        {
            iframesTimer += Time.deltaTime;

            if (iframesTimer > iframesMaxTime)
            {
                iframes = false;
                iframesTimer = 0;
            }
        }
    }

    void HandleRegen()
    {
        if (health < 100)
        {
            health += Time.deltaTime * healthRegen;
        }
        else
        {
            health = maxHealth;
        }

        if (stamina < 100)
        {
            stamina += Time.deltaTime * staminaRegen;
        }
        else
        {
            stamina = maxStamina;
        }

        if (health < 0)
        {
            health = 0;
        }

        if (stamina < 0)
        {
            stamina = 0;
        }
    }

}
