using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

public class Player : Character
{
    // Hey guys, artist here, (nathaniel huntington)
    // Had to make an event here
    // Dont mind me,
    //for thanks heres an unspaced comment

    public event Action<float> OnTakeDamage;

    // General player character variables and functions
    // Excludes combat-related things (cooldowns, attacks, abilities)

    public static Player instance;

    //[Tab("Main")]
    //[Header("Stats")]
    //public float fMaxHealth = 100.0f;
    //public float fHealth;
    //public float fMoveSpeed = 10.0f;

    [Tab("Movement")]
    public bool canMove = true;
    Vector3 moveVelocity = Vector3.zero;
    Vector3 camRotation = Vector3.zero;

    public float gravity = 9.81f;

    [SerializeField] private float coyoteTime = 0.25f;
    [SerializeField] private float coyoteTimer = 0.0f;

    public float acceleration = 4.0f;
    public AnimationCurve frictionCurve;
    public float stopFriction = 4.0f;

    public float airMod = 0.1f;

    //[Foldout("Dash Vars")]
    //[SerializeField] private float fDashDistance = 10.0f;
    //[SerializeField] private float fDashTime = 0.3f;
    //public float fDashCooldown = 3.0f;
    //public float fDashCDTimer = 0.0f;
    //public bool bCanDash = true;
    //
    //public event Action<float> OnDash;

    [Tab("Dev Mode")]
    public bool bImmortal = false;

    [Header("Keybinds")]
    [SerializeField] KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField] KeyCode knockbackKey = KeyCode.K;
    [SerializeField] KeyCode immortalKey = KeyCode.I;

    [Tab("References")]
    public GameObject body;
    public GameObject camTarget;
    public CharacterController charController;
    Animator playerAnim;
    public GameObject CMvcam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
        ProcessRotations();

        // Check for dash input
        //if (Input.GetKeyDown(dashKey) && bCanDash)
        //{
        //    Debug.Log("SCHWOOM DASH!");
        //    StartCoroutine(Dash());
        //}
        //
        //// --------------------- DEV KEYS ---------------------
        //if (Input.GetKeyDown(knockbackKey))
        //{
        //    StartCoroutine(Knockback(Vector3.zero, 5.0f));
        //}

        if (Input.GetKeyDown(immortalKey))
        {
            bImmortal = !bImmortal;
        }
    }

    /***********************************************
    * ProcessMovement: Processes character movement according to movement input.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    void ProcessMovement()
    {
        //playerAnim.SetBool("canMove", bCanMove);

        Vector3 moveInput = Vector3.zero;
        Vector3 playerRight = camTarget.transform.right;
        Vector3 playerForward = camTarget.transform.forward;
        
        playerRight.y = 0.0f;
        playerForward.y = 0.0f;

        playerRight.Normalize();
        playerForward.Normalize();

        // Get movement input
        moveInput.z += Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;   // Forwards
        moveInput.z -= Input.GetKey(KeyCode.S) ? 1.0f : 0.0f;   // Backwards
        moveInput.x += Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;   // Right
        moveInput.x -= Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;   // Left

        // Apply movement according to camera facing direction
        moveInput = playerRight * moveInput.x +
            playerForward * moveInput.z;


        // Ensures moving diagonally is not faster
        if (moveInput.sqrMagnitude > 1.0f)
        {
            moveInput.Normalize();
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0.0f)
        {
            moveVelocity.y = 12.0f; // Jump speed
            coyoteTimer = 0.0f;
        }

        float airMultiplier = (coyoteTimer > 0.2f) ? 1.0f : airMod;

        moveVelocity.y -= gravity * Time.deltaTime;
        float cacheY = moveVelocity.y;
        moveVelocity.y = 0.0f;

        moveVelocity += moveInput * airMultiplier * acceleration * Time.deltaTime;

        // Apply friction - more friction if no movement input
        float frictionVal = frictionCurve.Evaluate(moveVelocity.magnitude);
        float frictionMod = (moveInput.sqrMagnitude < 0.01f) ? stopFriction : 1.0f;
        moveVelocity -= moveVelocity.normalized * airMultiplier * 
            frictionVal * frictionMod * 
            acceleration * Time.deltaTime;

        moveVelocity.y = cacheY;

        Vector3 velocity = moveVelocity;
        velocity.x *= moveSpeed * speedMultiplier;
        velocity.z *= moveSpeed * speedMultiplier;

        charController.Move(velocity * Time.deltaTime);

        // Coyote time
        coyoteTimer -= Time.deltaTime;
        if ((charController.collisionFlags & CollisionFlags.Below) != 0)    // Grounded check
        //if (charController.isGrounded)    // Grounded check
        {
            coyoteTimer = coyoteTime;
            moveVelocity.y = -1.0f;
        }

        //if (canMove)
        //{
        //    charController.Move(moveVelocity * Time.deltaTime);
        //    velocity = charController.velocity.magnitude;
        //}

        // This is gravity
        //charController.Move(Vector3.up * -9.81f * Time.deltaTime * 2);

        // Update animator according to movement
        playerAnim.SetFloat("Velocity", charController.velocity.magnitude);
    }

    /***********************************************
    * ProcessRotations: Processes character and camera rotation based on mouse input.
    * @author: Justhine Nisperos
    * @parameter: 
    * @return: void
    ************************************************/
    void ProcessRotations()
    {
        camRotation = camTarget.transform.forward;

        // Reset Y vector to 0 to discount camera rotation
        // And prevent gravity from affecting player rotation
        //moveVelocity.y = 0;

        // Rotate player in desired direction
        if (moveVelocity != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(camRotation.x, 0.0f, camRotation.z));
            transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRot, 10.0f * Time.deltaTime);
        }

        // Reset cam target rotation and rotate to match camera rotation
        //camTarget.transform.rotation = camRotation;
        Vector3 rotDiff = CMvcam.transform.eulerAngles - camTarget.transform.eulerAngles;
        camTarget.transform.eulerAngles += rotDiff;
    }

    //public void TurnOffCanMove()
    //{
    //    canMove = false;
    //    moveVelocity = Vector3.zero;
    //    float velocity = 0.0f;
    //
    //    charController.Move(moveVelocity * moveSpeed * Time.deltaTime);
    //    velocity = charController.velocity.magnitude;
    //
    //    // Update animator according to movement
    //    playerAnim.SetFloat("Velocity", velocity);
    //}
}
