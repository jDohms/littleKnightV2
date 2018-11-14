using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController : MonoBehaviour
{
    public bool isFalling;
    public GameObject GC;
    Rigidbody rigidBody;
    Animator anim;
    CapsuleCollider capCol;
    Transform cam;
    Stats plStats;
    public bool dead;
    public bool steps;
    public bool canBackstab;


    public GameObject lockOnFX;
    public Transform camHolder;

    [SerializeField] float lockSpeed = 0.5f;
    [SerializeField] float normalSpeed = 0.8f;
    [SerializeField] float speed;

    [SerializeField] float turnSpeed = 5;

    Vector3 directionPos;
    Vector3 storeDir;

    [HideInInspector]
    public float horizontal;
    [HideInInspector]
    public float vertical;
    [HideInInspector]
    public bool rollInput;
    [HideInInspector]
    public bool attackInput;
    [HideInInspector]
    public bool attackInput2;
    [HideInInspector]
    public bool attackSFXInput;
    [HideInInspector]
    public bool attackSFXInputRelease;
    [HideInInspector]
    public bool blockInput;
    [HideInInspector]
    public bool parryInput;



    public bool lockTarget;
    int curTarget;
    bool changeTarget;

    float targetTurnAmount;
    float curTurnAmount;
    public bool canMove;
    public bool canTurn;
    public List<Transform> Enemies = new List<Transform>();

    public Transform cameraTarget;
    public float camTargetSpeed = 5;
    [HideInInspector]
    public Vector3 targetPos;

    void Start()
    {

        rigidBody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        camHolder = cam.parent.parent;
        capCol = GetComponent<CapsuleCollider>();
        SetupAnimator();

        GetComponent<PlayerAnimator>().enabled = true;
        plStats = GetComponent<Stats>();

    }

    void FixedUpdate()
    {
        CheckIfFalling();
        HandleInput();
        HandleCameraTarget();

        if (plStats.dead == true)
        {
            dead = true;
        }

        if (canMove)
        {
            if (!lockTarget)
            {
                speed = normalSpeed;
                HandleMovementNormal();
            }
            else
            {
                speed = lockSpeed;

                if (Enemies.Count > 0)
                {
                    HandleMovementLockOn();
                    HandleRotationOnLock();
                }
                else if (Enemies.Count == 0)
                {
                    lockTarget = false;
                    HandleMovementNormal();
                }
            }
        }


        if (horizontal != 0 || vertical != 0)
        {
            GC.SendMessage("PlaySteps");
            GC.SendMessage("PlayChain");
        }

        if (dead)
        {
            GC.SendMessage("PlayGameOver");
            GC.SendMessage("IvokeGameOver");
        }
    }

    void HandleInput()
    {
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");
        rollInput = CrossPlatformInputManager.GetButton("Fire3");
        storeDir = camHolder.right;
        // attackSFXInputRelease = CrossPlatformInputManager.GetMouseButtonUp(0);
        // attackSFXInput = CrossPlatformInputManager.GetMouseButtonDown(0);
        attackInput = CrossPlatformInputManager.GetButton("Attack");
        blockInput = CrossPlatformInputManager.GetButton("Fire4");

        ChangeTargetsLogic();
    }

    void ChangeTargetsLogic()
    {
        if (!lockTarget)
        {
            lockTarget = !lockTarget;
        }
        if (CrossPlatformInputManager.GetButton("Fire2") && lockTarget)
        {
            if (curTarget < Enemies.Count - 1)
            {
                curTarget++;
            }
            else
            {
                curTarget = 0;
            }
        }
    }

    void HandleMovementNormal()
    {
        canMove = anim.GetBool("CanMove");

        Vector3 dirForward = storeDir * horizontal;
        Vector3 dirSides = camHolder.forward * vertical;

        if (canMove)
            rigidBody.AddForce((dirForward + dirSides).normalized * speed / Time.deltaTime);

        directionPos = transform.position + (storeDir * horizontal) + (cam.forward * vertical);

        Vector3 dir = directionPos - transform.position;
        dir.y = 0;

        float angle = Vector3.Angle(transform.forward, dir);

        float animValue = Mathf.Abs(horizontal) + Mathf.Abs(vertical);


        animValue = Mathf.Clamp01(animValue);

        if (animValue > 0)

            anim.SetFloat("Forward", animValue);
        anim.SetBool("LockOn", false);

        if (horizontal != 0 || vertical != 0)
        {
            if (angle != 0 && canMove)
            {
                rigidBody.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
            }
        }

    }

    void HandleMovementLockOn()
    {
        Transform camHolder = cam.parent.parent;
        Vector3 camForward = Vector3.Scale(camHolder.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(camHolder.right, new Vector3(1, 0, 1)).normalized;
        Vector3 move = vertical * camForward + horizontal * cam.right;

        Vector3 moveForward = camForward * vertical;
        Vector3 moveSideways = camRight * horizontal;

        PlayerAnimator pl = GetComponent<PlayerAnimator>();
        AttackControl ac = GetComponent<AttackControl>();

        if (pl.rolling != true && ac.currentlyAttacking != true)
        {
            rigidBody.AddForce((moveForward + moveSideways).normalized * speed / Time.deltaTime);
        }

        ConvertMoveInputAndPassItToAnimator(move);
    }

    void ConvertMoveInputAndPassItToAnimator(Vector3 moveInput)
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        float turnAmount = localMove.x;
        float forwardAmount = localMove.z;

        if (turnAmount != 0)
            turnAmount *= 2;

        anim.SetBool("LockOn", true);
        anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Sideways", turnAmount, 0.1f, Time.deltaTime);
    }

    void HandleRotationOnLock()
    {
        PlayerAnimator pl = GetComponent<PlayerAnimator>();
        if (pl.rolling != true)
        {
            Vector3 lookPos = Enemies[curTarget].position;
            Vector3 lookDir = lookPos - transform.position;

            lookDir.y = 0;

            Quaternion rot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(rigidBody.rotation, rot, Time.deltaTime * turnSpeed);
        }
    }


    void HandleCameraTarget()
    {
        if (!lockTarget)
        {
            targetPos = transform.position;
        }
        else
        {
            if (Enemies.Count > 0)
            {
                Vector3 direction = Enemies[curTarget].position - transform.position;
                direction.y = 0;

                float distance = Vector3.Distance(transform.position, Enemies[curTarget].position);

                targetPos = direction.normalized * distance;

                targetPos += transform.position;

                if (distance > 40)
                {
                    lockTarget = false;
                }
            }

            else if (Enemies.Count <= 0)
            {
                lockTarget = false;
                cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPos,
                                     Time.deltaTime * camTargetSpeed);
            }
        }

        cameraTarget.position = Vector3.Lerp(cameraTarget.position, targetPos,
                                             Time.deltaTime * camTargetSpeed);
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

    void CheckIfFalling()
    {
        if (rigidBody.velocity.y > -0.5)
        {
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreLayerCollision(11, 11);
        if (other.gameObject.name == "BackStabHitbox")
        {
            canBackstab = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Physics.IgnoreLayerCollision(11, 11);
        if (other.gameObject.name == "BackStabHitbox")
        {
            canBackstab = false;
        }
    }

}
