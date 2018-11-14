using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    PlayerController plController;

    public GameObject GC;

    public float rollSpeed = 5;

    float horizontal;
    float vertical;
    bool rollInput;

    public bool rolling;
    bool hasRolled;

    Vector3 directionPos;
    Vector3 storeDir;
    Transform camHolder;
    Rigidbody rigBody;
    Animator anim;
    Stats plStats;


    public bool hasDirection;

    Vector3 dirForward;
    Vector3 dirSides;
    Vector3 dir;

    void Start ()
    {
        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
        plController = GetComponent<PlayerController>();
        camHolder = plController.camHolder;
        rigBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        plStats = GetComponent<Stats>();
    }

    void FixedUpdate ()
    {
        this.rollInput = plController.rollInput;
        this.horizontal = plController.horizontal;
        this.vertical = plController.vertical;
        storeDir = camHolder.right;

        if (rollInput && plStats.stamina > 40)
        {
            if (!rolling)
            {
                plController.canMove = false;
                rolling = true;
            }
        }

        if (rolling)
        {
            if (!hasDirection)
            {
                dirForward = storeDir * horizontal;
                dirSides = camHolder.forward * vertical;

                directionPos = transform.position + (storeDir * horizontal) + (camHolder.forward * vertical);
                dir = directionPos - transform.position;
                dir.y = 0;

                anim.SetTrigger("Roll");
                hasDirection = true;
                plStats.stamina -= 40;
            }

            rigBody.AddForce((dirForward + dirSides).normalized * rollSpeed / Time.deltaTime);

            float angle = Vector3.Angle(transform.forward, dir);

            if (angle != 0)
            {
                rigBody.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(dir), 15 * Time.deltaTime);
            }


        }
    }
}
