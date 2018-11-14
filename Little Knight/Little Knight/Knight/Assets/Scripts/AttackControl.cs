using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControl : MonoBehaviour {

    public AudioSource PlAudio;
    Animator anim;
    Rigidbody rigBody;
    PlayerController plControl;
    Stats plStats;
    public bool stab;

    bool attackInput;
    bool blockInput;
    public bool currentlyAttacking;
    public bool blocking;
    bool decreaseStamina;

    public float damCollMaxTime = 1;
    float damCollTimer;
    public GameObject damageCollider;
    public GameObject GC;

	void Start ()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        anim = GetComponent<Animator>();
        plControl = GetComponent<PlayerController>();
        plStats = GetComponent<Stats>();
    }

    void FixedUpdate ()
    {
        if (currentlyAttacking)
        {
            PlAudio.Play();
        }

        UpdateInput();
        HandleAttacks();
        HandleBlock();
    }

    void UpdateInput()
    {
        this.attackInput = plControl.attackInput;
        this.blockInput = plControl.blockInput;
    }

    void HandleAttacks()
    {
        if (currentlyAttacking)
        {
            damageCollider.SetActive(true);

            damCollTimer += Time.deltaTime;

            if (damCollTimer > damCollMaxTime)
            {
                damageCollider.SetActive(true);
                damCollTimer = 0;
            }
        }
        else
        {
            damageCollider.SetActive(false);
        }

        if (attackInput && !blocking )
        {
            plControl.canMove = false;
            currentlyAttacking = true;


            if (!decreaseStamina)
            {
                plStats.stamina -= 20;
                decreaseStamina = true;
            }

            StartCoroutine("InitiateAttack");
        }
        else
        {
            decreaseStamina = false;
        }
    }

    void HandleBlock()
    {
        if (blockInput)
        {
            blocking = true;
            plStats.blocking = true;
            anim.SetBool("Block", true);
        }
        else
        {
            blocking = false;
            plStats.blocking = false;
            anim.SetBool("Block", false);
        }
    }
  

    IEnumerator InitiateAttack()
    {

        if (plStats.stamina > 20 && attackInput && !plControl.canBackstab)
        {
            yield return new WaitForEndOfFrame();
            anim.SetBool("Attack", true);
        }
        else if (plControl.canBackstab == true)
        {
            yield return new WaitForEndOfFrame();
            anim.SetTrigger("Stab");
            stab = true;
        }
    }


    IEnumerator CloseAttack()
    {
        yield return new WaitForSeconds(1);
        damageCollider.SetActive(false);
        stab = false;
    }

}

