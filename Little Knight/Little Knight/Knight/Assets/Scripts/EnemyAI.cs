using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject deathFX;
    bool isDeathFXSpawned = false;
    Stats stats;
    [HideInInspector]
    public NavMeshAgent agent;
    Animator anim;

    Rigidbody rigidBody;
    public Transform curTarget;
    public float range = 3;
    float runDistance;
    public bool currentlyAttacking;
    public float attackRate = 2;
    float attTimer;
    bool attackOnce;
    bool stopRotating;
    float attackCurve;
    public GameObject damageTrigger;
    public GameObject GC;
    public float turnSpeed;


    [Header("Ranged")]
    public bool isRanged;
    public GameObject magic;
    public Transform bulletSpawn;
    public Transform bulletSpawn2;
    public Transform bulletSpawn3;
    public float fireRate;
    private float nextFire;
    public PlayerController plControl;


    public bool canAttack;
    public List<Transform> Enemies = new List<Transform>();


    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        stats = GetComponent<Stats>();
        agent = GetComponent<NavMeshAgent>();
        SetupAnimator();
        agent.stoppingDistance = range;
    }

    void Update()
    {
        if (!stats.dead)
        {
            if (!currentlyAttacking)
            {
                MovementHandler();
            }
            AttackHandler();
        }
        else
        {
            plControl.Enemies.Remove(gameObject.transform);
            if (!isDeathFXSpawned)
            {
                Instantiate(deathFX, transform.position, transform.rotation);
                isDeathFXSpawned = true;
            }
                Destroy(gameObject, 1.0f);

        }

        foreach (bool currentlyAttacking in Enemies)
        {
            canAttack = false;
        }
    }

    void MovementHandler()
    {
        if (curTarget != null && stats.health > 10)
        {
            agent.SetDestination(curTarget.position);

            Vector3 relDirection = transform.InverseTransformDirection(agent.desiredVelocity);

            anim.SetFloat("Movement", relDirection.z, 0.5f, Time.deltaTime);

            float distance = Vector3.Distance(transform.position, curTarget.position);

            Vector3 lookPos = curTarget.position;
            Vector3 lookDir = lookPos - transform.position;

            lookDir.y = 0;

            Quaternion rot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(rigidBody.rotation, rot, Time.deltaTime * turnSpeed);

            if (distance <= range)
            {
                attTimer += Time.deltaTime;

                if (attTimer > attackRate)
                {
                    currentlyAttacking = true;
                    attTimer = 0;
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

                    if (isRanged && Time.time > nextFire)
                    {
                        StartCoroutine("Fire");
                        nextFire = Time.time + fireRate;
                    }

                    else if (!attackOnce && !isRanged)
                    {
                        agent.isStopped = true;
                        //GC.SendMessage("PlayAttack");
                        anim.SetBool("Attack", true);
                        StartCoroutine("CloseAttack");
                        attackOnce = true;
                    }
                    else attackOnce = false;
                }

            }

            if (!isRanged)
            {

                attackCurve = anim.GetFloat("attackCurve");

                if (attackCurve > .5f)
                {
                    stopRotating = true;

                    if (attackCurve > .98f)
                    {
                        damageTrigger.SetActive(true);
                    }
                    else
                    {
                        if (damageTrigger.activeInHierarchy)
                        {
                            damageTrigger.SetActive(false);
                        }
                    }
                }
            }
        }
    }
        IEnumerator CloseAttack()
        {
            yield return new WaitForSeconds(8);
            anim.SetBool("Attack", false);
            attackOnce = false;
            agent.isStopped = false;
            currentlyAttacking = false;
            stopRotating = false;
            damageTrigger.SetActive(false);
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

        IEnumerator Fire()
        {
            anim.SetTrigger("Magic");
            GC.SendMessage("PlayMagic");

            yield return new WaitForSeconds(1);
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

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 5;
            bullet2.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 5;
            bullet3.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 5;

            Destroy(bullet, 9.0f);
            Destroy(bullet2, 9.0f);
            Destroy(bullet3, 9.0f);

        }
}

