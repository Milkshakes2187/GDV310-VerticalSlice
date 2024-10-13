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

    private float gravity = 9.81f;

    private float coyoteTime = 0.25f;
    private float coyoteTimer = 0.0f;

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

    void ProcessMovement()
    {
        //playerAnim.SetBool("canMove", bCanMove);

        Vector3 moveInput = Vector3.zero;

        // Get movement input
        moveInput.z += Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;   // Forwards
        moveInput.z -= Input.GetKey(KeyCode.S) ? 1.0f : 0.0f;   // Backwards
        moveInput.x += Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;   // Right
        moveInput.x -= Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;   // Left

        // Apply movement according to camera facing direction
        moveInput = camTarget.transform.right * moveInput.x +
            camTarget.transform.forward * moveInput.z;


        // Coyote time
        coyoteTimer -= Time.deltaTime;
        if ((charController.collisionFlags & CollisionFlags.Below) != 0)    // Grounded check
        {
            coyoteTimer = coyoteTime;
            moveVelocity.y = -1.0f;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0.0f)
        {
            moveVelocity.y = 10.0f; // Jump speed
            coyoteTimer = 0.0f;
        }

        moveVelocity.x = moveInput.x * moveSpeed * speedMultiplier;
        moveVelocity.y -= gravity * Time.deltaTime;
        moveVelocity.z = moveInput.z * moveSpeed * speedMultiplier;

        float velocity = 0.0f;
        if (canMove)
        {
            charController.Move(moveVelocity * Time.deltaTime);
            velocity = charController.velocity.magnitude;
        }

        // This is gravity
        //charController.Move(Vector3.up * -9.81f * Time.deltaTime * 2);

        // Update animator according to movement
        playerAnim.SetFloat("Velocity", velocity);
    }

    void ProcessRotations()
    {
        camRotation = camTarget.transform.forward;

        // Reset Y vector to 0 to discount camera rotation
        // And prevent gravity from affecting player rotation
        moveVelocity.y = 0;

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

    public void Jump()
    {

    }

    //IEnumerator Dash()
    //{
    //    // Disable dash ability
    //    bCanDash = false;
    //    OnDash?.Invoke(fDashTime);
    //    
    //    // TEDDY SOUND Dash sound
    //    if (AudioLibrary.instance.audioDash.clip)
    //    {
    //        AudioLibrary.instance.audioDash.PlayOneShot(AudioLibrary.instance.audioDash.clip, 1);
    //    }
    //    
    //    // Turn player invisible & disable controller for manual movement
    //    body.SetActive(false);
    //    charController.enabled = false;
    //    
    //    // LERP player to dash position over the dash time
    //    float fElapsedTime = 0.0f;
    //    Vector3 origPos = transform.position;
    //    Vector3 targetPos = Vector3.zero;
    //    if (moveVelocity != Vector3.zero)
    //    {
    //        // Dash in the player's movement input direction
    //        targetPos = origPos + moveVelocity * fDashDistance;
    //    }
    //    else
    //    {
    //        // No direction input - Dash in direction character is facing
    //        targetPos = origPos + body.transform.forward * fDashDistance;
    //    }
    //    
    //    
    //    while (fElapsedTime < fDashTime)
    //    {
    //        transform.position = Vector3.Lerp(origPos, targetPos, fElapsedTime / fDashTime);
    //        fElapsedTime += Time.deltaTime;
    //    
    //        yield return null;
    //    }
    //    
    //    // Turn player visible & reenable controller
    //    body.SetActive(true);
    //    charController.enabled = true;
    //    
    //    // Wait for dash cooldown before reenabling dash
    //    while (fDashCDTimer <= fDashCooldown)
    //    {
    //        fDashCDTimer += Time.deltaTime;
    //        yield return null;
    //    }
    //    
    //    bCanDash = true;
    //    fDashCDTimer = 0.0f;
    //}

    //IEnumerator Knockback(Vector3 _source, float _fDist)
    //{
    //    bCanMove = false;
    //    charController.enabled = false;
    //
    //    // Get direction of player from knockback source & zero out y-value
    //    Vector3 forceDir = transform.position - _source;
    //    forceDir.Normalize();
    //    forceDir.y = 0.0f;
    //
    //    Vector3 origPos = transform.position;
    //    Vector3 targetPos = origPos + forceDir * _fDist;
    //    float fKnockbackTime = 0.1f;
    //    float fElapsedTime = 0.0f;
    //
    //    while (fElapsedTime < fKnockbackTime)
    //    {
    //        transform.position = Vector3.Lerp(origPos, targetPos, fElapsedTime / fKnockbackTime);
    //        fElapsedTime += Time.deltaTime;
    //
    //        yield return null;
    //    }
    //    
    //    // Apply movement cooldown after knockback
    //    yield return new WaitForSeconds(0.5f);
    //
    //    charController.enabled = true;
    //    bCanMove = true;
    //}

    public void Step()
    {
        // TEDDY SOUND Footsteps
        if (AudioLibrary.instance.audioStep.clip)
        {
            AudioLibrary.instance.audioStep.PlayOneShot(AudioLibrary.instance.audioStep.clip, 1);
        }
    }
}
