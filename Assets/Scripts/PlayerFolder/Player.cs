using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

public class Player : MonoBehaviour
{
    // Hey guys, artist here, (nathaniel huntington)
    // Had to make an event here
    // Dont mind me,
    //for thanks heres an unspaced comment

    public event Action<float> OnTakeDamage;

    // General player character variables and functions
    // Excludes combat-related things (cooldowns, attacks, abilities)

    public static Player instance;

    [Tab("Main")]
    [Header("Stats")]
    public float fMaxHealth = 100.0f;
    public float fHealth;
    public float fMoveSpeed = 10.0f;
    public bool bCanMove = true;
    Vector3 v3MoveVec = Vector3.zero;
    Quaternion qCamRotation = Quaternion.identity;

    [Foldout("Dash Vars")]
    [SerializeField] private float fDashDistance = 10.0f;
    [SerializeField] private float fDashTime = 0.3f;
    public float fDashCooldown = 3.0f;
    public float fDashCDTimer = 0.0f;
    public bool bCanDash = true;

    public event Action<float> OnDash;

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
        fHealth = fMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
        ProcessRotations();

        // Check for dash input
        if (Input.GetKeyDown(dashKey) && bCanDash)
        {
            Debug.Log("SCHWOOM DASH!");
            StartCoroutine(Dash());
        }

        // --------------------- DEV KEYS ---------------------
        if (Input.GetKeyDown(knockbackKey))
        {
            StartCoroutine(Knockback(Vector3.zero, 5.0f));
        }

        if (Input.GetKeyDown(immortalKey))
        {
            bImmortal = !bImmortal;
        }
    }

    public void TurnOffCanMove()
    {
        bCanMove = false;
        v3MoveVec = Vector3.zero;
        float velocity = 0.0f;
        
        charController.Move(v3MoveVec * fMoveSpeed * Time.deltaTime);
        velocity = charController.velocity.magnitude;
        

        // Update animator according to movement
        playerAnim.SetFloat("Velocity", velocity);
    
    }

    void ProcessMovement()
    {
        //playerAnim.SetBool("canMove", bCanMove);

        v3MoveVec = Vector3.zero;

        // Forward and backward
        if (Input.GetKey(KeyCode.W))
        {
            v3MoveVec.z += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            v3MoveVec.z -= 1.0f;
        }

        // Left and right
        if (Input.GetKey(KeyCode.A))
        {
            v3MoveVec.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            v3MoveVec.x += 1.0f;
        }

        // Apply movement according to camera facing direction
        v3MoveVec = v3MoveVec.normalized;
        v3MoveVec = camTarget.transform.right * v3MoveVec.x +
            camTarget.transform.forward * v3MoveVec.z;

        float velocity = 0.0f;
        if (bCanMove)
        {
            charController.Move(v3MoveVec * fMoveSpeed * Time.deltaTime);
            velocity = charController.velocity.magnitude;
        }

        // This is gravity
        charController.Move(Vector3.up * -9.81f * Time.deltaTime * 2);

        // Update animator according to movement
        playerAnim.SetFloat("Velocity", velocity);
    }

    void ProcessRotations()
    {
        qCamRotation = camTarget.transform.rotation;

        // Reset Y vector to 0 to discount camera rotation
        // And prevent gravity from affecting player rotation
        v3MoveVec.y = 0;

        // Rotate player in desired direction
        if (v3MoveVec != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(v3MoveVec);
            transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRot, 10.0f * Time.deltaTime);
        }

        // Reset cam target rotation and rotate to match camera rotation
        camTarget.transform.rotation = qCamRotation;
        Vector3 rotDiff = CMvcam.transform.eulerAngles - camTarget.transform.eulerAngles;
        camTarget.transform.eulerAngles += rotDiff;
    }

    IEnumerator Dash()
    {
        // Disable dash ability
        bCanDash = false;
        OnDash?.Invoke(fDashTime);

        // TEDDY SOUND Dash sound
        if (AudioLibrary.instance.audioDash.clip)
        {
            AudioLibrary.instance.audioDash.PlayOneShot(AudioLibrary.instance.audioDash.clip, 1);
        }

        // Turn player invisible & disable controller for manual movement
        body.SetActive(false);
        charController.enabled = false;

        // LERP player to dash position over the dash time
        float fElapsedTime = 0.0f;
        Vector3 origPos = transform.position;
        Vector3 targetPos = Vector3.zero;
        if (v3MoveVec != Vector3.zero)
        {
            // Dash in the player's movement input direction
            targetPos = origPos + v3MoveVec * fDashDistance;
        }
        else
        {
            // No direction input - Dash in direction character is facing
            targetPos = origPos + body.transform.forward * fDashDistance;
        }


        while (fElapsedTime < fDashTime)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, fElapsedTime / fDashTime);
            fElapsedTime += Time.deltaTime;

            yield return null;
        }

        // Turn player visible & reenable controller
        body.SetActive(true);
        charController.enabled = true;

        // Wait for dash cooldown before reenabling dash
        while (fDashCDTimer <= fDashCooldown)
        {
            fDashCDTimer += Time.deltaTime;
            yield return null;
        }

        bCanDash = true;
        fDashCDTimer = 0.0f;
    }

    IEnumerator Knockback(Vector3 _source, float _fDist)
    {
        bCanMove = false;
        charController.enabled = false;

        // Get direction of player from knockback source & zero out y-value
        Vector3 forceDir = transform.position - _source;
        forceDir.Normalize();
        forceDir.y = 0.0f;

        Vector3 origPos = transform.position;
        Vector3 targetPos = origPos + forceDir * _fDist;
        float fKnockbackTime = 0.1f;
        float fElapsedTime = 0.0f;

        while (fElapsedTime < fKnockbackTime)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, fElapsedTime / fKnockbackTime);
            fElapsedTime += Time.deltaTime;

            yield return null;
        }
        
        // Apply movement cooldown after knockback
        yield return new WaitForSeconds(0.5f);

        charController.enabled = true;
        bCanMove = true;
    }

    public void TakeDamage(float _dmg)
    {
        fHealth -= _dmg;
        OnTakeDamage?.Invoke(_dmg);

        if (fHealth <= 0 && !bImmortal)
        {
            GameManager.instance.GameOver(false);
        }
    }

    public void TempHeal(float _health)
    {
        fHealth += _health;

        if (fHealth >= fMaxHealth)
        {
            fHealth = fMaxHealth;
        }
    }

    public void Step()
    {
        // TEDDY SOUND Footsteps
        if (AudioLibrary.instance.audioStep.clip)
        {
            AudioLibrary.instance.audioStep.PlayOneShot(AudioLibrary.instance.audioStep.clip, 1);
        }
    }
}