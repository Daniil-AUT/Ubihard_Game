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

    private Player playerScript;
    private bool isDead = false;

    void Start()
    {
        currentSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
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

        // Disable CharacterController to prevent further movement
        controller.enabled = false;

        // Zero out velocity and stop any movement
        velocity = Vector3.zero;

        // Play the death animation
        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

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
