using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour {

    public GameObject GC;
    Stats stats;
    NavMeshAgent agent;
    Animator anim;

    public Transform curTarget;
    public float meleeRange = 3;
    public bool currentlyAttacking;
    public float attackRate = 2;
    float attTimer;
    bool attackOnce;
    bool stopRotating;
    float attackCurve;
    public GameObject damageTrigger;

    public Transform torso;
    public GameObject shockwave;
    public GameObject chargeWave;
    public GameObject chargeProjectile;
    public GameObject chargeBomb;
    public float bombRange = 10;
    public float magicRange = 6;
    public GameObject magic;
    public GameObject bomb;
    public Transform bulletSpawn;
    public Transform bulletSpawn2;
    public Transform bulletSpawn3;
    public float fireRate;
    private float nextFire;



    void Start ()
    {
        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        SetupAnimator();
        agent.stoppingDistance = meleeRange;
	}
	
	void Update ()
    {
        if (!stats.dead)
        {
            if (!stats.dead)
            {
                if (!currentlyAttacking)
                {
                    MovementHandler();
                }
                AttackHandler();
            }
        }
        else
        {
            GC.SendMessage("IvokeWinner");
        }
    }

    void MovementHandler()
    {
        if (curTarget != null)
        {
            agent.SetDestination(curTarget.position);

            Vector3 relDirection = transform.InverseTransformDirection(agent.desiredVelocity);

            anim.SetFloat("Movement", relDirection.z, 0.5f, Time.deltaTime);

            float distance = Vector3.Distance(transform.position, curTarget.position);

            if (distance <= meleeRange && distance < bombRange && distance < magicRange && Time.time > nextFire)
            {
                Wave();
                nextFire = Time.time + attackRate;
            }

            else if (distance <= bombRange && distance > meleeRange && distance > magicRange && Time.time > nextFire)
            {
                {
                    Bomb();
                    nextFire = Time.time + fireRate;
                }

            }

            else if (distance <= magicRange && distance > meleeRange && distance < bombRange && Time.time > nextFire)
            {
                {
                    Fire();
                    nextFire = Time.time + fireRate;
                }

            }

        }
    }
    void AttackHandler()
    {

            if (currentlyAttacking)
            {
                if (!stopRotating)
                {
                    Vector3 dir = curTarget.position - transform.position;
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5);

                    float angle = Vector3.Angle(transform.forward, dir);

                    if (angle < 5)
                { 
                    
                     if (!attackOnce)
                        {
                        StartCoroutine("StartAttack");
                        StartCoroutine("CloseAttack");
                        }
                        else attackOnce = false;
                    }
                
            }
        }
    }

    IEnumerator StartAttack()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(2);
        anim.SetBool("Attack", true);
        attackOnce = true;
    }

    IEnumerator CloseAttack()
    {
        yield return new WaitForSeconds(8);
        anim.SetBool("Attack", false);
        attackOnce = false;
        agent.isStopped = false;
        currentlyAttacking = false;
        stopRotating = false;
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    void Fire()
    {
        StartCoroutine("StartProjectile");
    }

    void Bomb()
    {
        StartCoroutine("StartBomb");
    }

    void Wave()
    {
        StartCoroutine("StartWave");
    }

    IEnumerator StartWave()
    {
        agent.isStopped = true;
        var charge = (GameObject)Instantiate(
         chargeWave,
         torso.position,
         torso.rotation);
         Destroy(charge, 20f);

        anim.SetTrigger("Wave");
        yield return new WaitForSeconds(3);
        var wav = (GameObject)Instantiate(
            shockwave,
            torso.position,
            transform.rotation);
        Destroy(wav, 2.0f);
        StartCoroutine("CloseMagic");
    }

    IEnumerator StartProjectile()
    {
        agent.isStopped = true;
        var charge = (GameObject)Instantiate(
         chargeProjectile,
         torso.position,
         torso.rotation);
        Destroy(charge, 2.0f);

        anim.SetTrigger("Magic");
        yield return new WaitForSeconds(0.5f);

        var bullet = (GameObject)Instantiate(
            magic,
            bulletSpawn.position,
            bulletSpawn.rotation);

        var bullet2 = (GameObject)Instantiate(
        magic,
        bulletSpawn2.position,
        bulletSpawn2.rotation);
        var bullet3 = (GameObject)Instantiate(
        magic,
        bulletSpawn3.position,
        bulletSpawn3.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 12;
        bullet2.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 12;
        bullet3.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 12;

        StartCoroutine("CloseMagic");

        Destroy(bullet, 9.0f);
        Destroy(bullet2, 9.0f);
        Destroy(bullet3, 9.0f);
    }

    IEnumerator CloseMagic()
    {
        yield return new WaitForSeconds(1);
        agent.isStopped = false;
    }

    IEnumerator StartBomb()
    {
        agent.isStopped = true;

        var charge = (GameObject)Instantiate(
        chargeBomb,
        torso.position,
        torso.rotation);
        Destroy(charge, 2.0f);

        anim.SetTrigger("Bomb");
        yield return new WaitForSeconds(2);

        var bullet = (GameObject)Instantiate(
            bomb,
            bulletSpawn.position,
            bulletSpawn.rotation);

        var bullet2 = (GameObject)Instantiate(
        bomb,
        bulletSpawn2.position,
        bulletSpawn2.rotation);
        var bullet3 = (GameObject)Instantiate(
        bomb,
        bulletSpawn3.position,
        bulletSpawn3.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 15;
        bullet2.GetComponent<Rigidbody>().velocity = bullet.transform.up * 15;
        bullet3.GetComponent<Rigidbody>().velocity = bullet.transform.up * 15;

        StartCoroutine("CloseMagic");

        Destroy(bullet, 9.0f);
        Destroy(bullet2, 9.0f);
        Destroy(bullet3, 9.0f);
    }

}
