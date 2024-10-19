using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public Transform cam;
    private CharacterController controller;
    private Animator anim;
    private PlayerCrouch playerCrouch; 
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector3 deathPosition;
    public Vector2 movement;
    public float walkSpeed;
    public float sprintSpeed;
    public bool sprinting;
    public float currentSpeed;
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
    private bool isSpeedBoosted = false; 
    private Player playerScript;
    private bool isDead = false;

    void Start()
    {
        currentSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        playerCrouch = GetComponent<PlayerCrouch>();
        if (anim == null)
        {
            Debug.LogError("Animator component not found on this GameObject or its children.");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerScript = GetComponent<Player>();
        if (playerScript == null)
        {
            Debug.LogError("Player script not found on this GameObject.");
        }
    }

    void Update()
    {
        if (isDead) return;

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, 1);
        anim.SetBool("IsGrounded", isGrounded);

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
                if (Input.GetKeyDown(KeyCode.LeftShift) && !playerCrouch.isCrouching)
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump && !isDodging && !isAttacking && !isInCombat && !playerCrouch.isCrouching)

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

        // Check for death condition
        if (playerScript != null && playerScript.currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        if (playerScript != null)
        {
            playerScript.TakeDamage(damage);
        }
    }

    private IEnumerator HandleJump()
    {
        isJumping = true;
        float jumpStartTime = Time.time;

        while (Time.time < jumpStartTime + jumpDuration)
        {
            float t = (Time.time - jumpStartTime) / jumpDuration;
            velocity.y = jumpCurve.Evaluate(t) * jumpHeight;
            yield return null;
        }

        isJumping = false;
    }

    public void Dodge()
    {
        if (!isDodging && !isAttacking)
        {
            isDodging = true;
            currentSpeed = sprintSpeed;
            anim.SetTrigger("Dodge");

            StartCoroutine(StopDodging());
        }
    }

    // Apply speed boost
    public void ApplySpeedBoost(float amount)
    {
        if (isSpeedBoosted) return; // Prevent stacking

        isSpeedBoosted = true; // Set flag
        walkSpeed += amount;
        sprintSpeed += amount;
        currentSpeed = walkSpeed; // Reset current speed to walk speed
        Debug.Log($"Speed increased by {amount}. New walk speed: {walkSpeed}, New sprint speed: {sprintSpeed}");

        // Reset speed after duration (optional)
        StartCoroutine(ResetSpeedAfterDuration(5f)); 
    }

    private IEnumerator ResetSpeedAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        walkSpeed -= 5; 
        sprintSpeed -= 5;
        isSpeedBoosted = false; 
        Debug.Log($"Speed reset. Walk speed: {walkSpeed}, Sprint speed: {sprintSpeed}");
    }

    private IEnumerator StopDodging()
    {
        yield return new WaitForSeconds(0.5f);
        isDodging = false;
        currentSpeed = walkSpeed;
    }

    public void Attack()
    {
        if (!isAttacking && !isDodging)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");

            StartCoroutine(StopAttacking());
        }
    }

    private IEnumerator StopAttacking()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    private void Die()
    {
        isDead = true;

        // Zero out velocity and stop any movement
        velocity = Vector3.zero;

    }

    public void ResetController()
    {
        isDead = false;
        velocity = Vector3.zero;
        isJumping = false;
        isDodging = false;
        isAttacking = false;
        currentSpeed = walkSpeed;
        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
    }
}
