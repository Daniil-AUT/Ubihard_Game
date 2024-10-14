using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;
    private CharacterController controller;
    private Animator anim;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Vector2 movement;
    public float walkSpeed;
    public float sprintSpeed;
    public bool sprinting;
    private float currentSpeed;
    private Player playerScript;
    private bool isDead = false;
    public float jumpHeight;
    public float gravity;
    public AnimationCurve jumpCurve;
    bool isGrounded;
    bool canJump = true;
    public bool isJumping = false;
    public Vector3 velocity;
    public bool isDodging = false;
    public bool isAttacking = false;
    public bool isInCombat = false;
    float jumpTime;
    public float jumpDuration = 1f;

    // Health variables
    public float maxHealth = 100f; // Set maximum health
    private float currentHealth;

    // Reference to GameOverManager
    public GameOverManager gameOverManager;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
        currentSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerScript = GetComponent<Player>();
        if (playerScript == null)
        {
            Debug.LogError("Player script not found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, 1);
        anim.SetBool("IsGrounded", isGrounded);
        if (isDead) return;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
            anim.SetBool("IsJumping", false);
        }

        if (!isAttacking && !isInCombat)
        {
            if (isDodging)
            {
                currentSpeed = walkSpeed;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    currentSpeed = sprintSpeed;
                    sprinting = true;
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    currentSpeed = walkSpeed;
                    sprinting = false;
                }
            }

            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);

                anim.SetFloat("Speed", sprinting ? 2 : 1);
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

        // Handle jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump && !isDodging && !isAttacking)
        {
            StartCoroutine(HandleJump());
            anim.SetTrigger("Jump");
            anim.SetBool("IsJumping", true);
        }

        if (velocity.y > -20 && !isJumping)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        // Check if health is zero to trigger death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        if (isDead) return; // Prevent taking damage when dead

        currentHealth -= damage;
        Debug.Log($"Current Health: {currentHealth}");

        // Check if player is dead
        if (currentHealth <= 0)
        {
            anim.SetTrigger("Die"); // Play death animation
            Die();
        }
    }

    // Method to handle player death
    public void Die()
    {
        isDead = true;
        playerScript.Die(); // Call player's Die method to handle death animation and logic
        gameOverManager.TriggerGameOver(); // Call game over manager
    }

    private IEnumerator HandleJump()
    {
        isJumping = true;
        float jumpStartTime = Time.time;

        while (Time.time < jumpStartTime + jumpDuration)
        {
            float t = (Time.time - jumpStartTime) / jumpDuration;
            velocity.y = jumpCurve.Evaluate(t) * jumpHeight; // Adjust height using the jump curve
            yield return null;
        }

        isJumping = false;
    }

    // Method to handle dodging
    public void Dodge()
    {
        if (!isDodging && !isAttacking)
        {
            isDodging = true;
            currentSpeed = sprintSpeed; // Speed up while dodging
            anim.SetTrigger("Dodge");

            StartCoroutine(StopDodging());
        }
    }

    private IEnumerator StopDodging()
    {
        yield return new WaitForSeconds(0.5f); // Adjust this based on your dodge duration
        isDodging = false;
        currentSpeed = walkSpeed; // Reset speed after dodging
    }

    // Method to handle attacking
    public void Attack()
    {
        if (!isAttacking && !isDodging)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
            // Handle attack logic here

            StartCoroutine(StopAttacking());
        }
    }

    private IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(1f); // Adjust this based on your attack animation length
        isAttacking = false;
    }
}
