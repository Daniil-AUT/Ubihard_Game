using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;

    CharacterController controller;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private Animator anim;

    Vector2 movement;
    public float walkSpeed;
    public float sprintSpeed;
    bool sprinting;
    private float currentSpeed;

    public float jumpHeight;
    public float gravity;
    bool isGrounded; 
    Vector3 velocity;

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

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            sprinting = true;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
            sprinting = false;
        }

        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);

            if(sprinting)
            {
                anim.SetFloat("Speed", 2);
            } 
            else 
            {
                anim.SetFloat("Speed", 1);
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * gravity); 
        }

        if(velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
