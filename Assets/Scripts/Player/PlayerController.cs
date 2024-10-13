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

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    IEnumerator HandleJump()
    {
        canJump = false;
        isJumping = true;
        jumpTime = 0f;

        while (jumpTime < jumpDuration)
        {
            float normalizedTime = jumpTime / jumpDuration;
            velocity.y = jumpCurve.Evaluate(normalizedTime) * jumpHeight;
            jumpTime += Time.deltaTime;
            yield return null;
        }

        velocity.y = 0;
        yield return new WaitForSeconds(0.3f);
        canJump = true;
        isJumping = false;
    }

}
