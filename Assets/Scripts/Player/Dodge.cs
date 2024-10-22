using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] AnimationCurve dodgeCurve;

    private PlayerController playerController;  
    private Animator anim;
    private CharacterController characterController;
    private PlayerAttack playerAttack;
    private PlayerCrouch playerCrouch;

    public bool isDodging = false;
    private float dodgeTimer;
    private float dodgeDistance = 2f;
    private Vector2 dodgeDirectionInput; 
    private Vector3 dodgeDirection;

    void Start()
    {
        playerController = GetComponent<PlayerController>(); 
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        playerAttack = GetComponent<PlayerAttack>(); 
        playerCrouch = GetComponent<PlayerCrouch>();

        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.keys.Length - 1];
        dodgeTimer = dodge_lastFrame.time;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !playerAttack.isAttacking && !isDodging && !playerCrouch.isCrouching)
        {
            if (playerController.isInCombat)
            {
                StartCoroutine(DirectionalDodging(dodgeDirectionInput, dodgeDistance));  
            }
            else if (playerController.movement.magnitude > 0)
            {
                StartCoroutine(Dodging());
            }
        }
    }

    public void SetDodgeDirection(Vector2 movementInput)
    {
        dodgeDirectionInput = movementInput;
    }

    IEnumerator Dodging()
    {
        isDodging = true;
        playerController.isDodging = true;  

        Player player = GetComponent<Player>();
        if (player != null)
        {
            player.SetInvincible(true);
        }

        float timer = 0;
        anim.SetTrigger("Dodge");

        dodgeDirection = new Vector3(playerController.movement.x, 0, playerController.movement.y).normalized;

        while (timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            
            Vector3 dir = (dodgeDirection * speed) + (Vector3.up * playerController.velocity.y);
            characterController.Move(dir * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        if (player != null)
        {
            player.SetInvincible(false);
        }

        isDodging = false;
        playerController.isDodging = false;  
    }

    IEnumerator DirectionalDodging(Vector2 directionInput, float distance)
{
    isDodging = true;
    playerController.isDodging = true;  

    Player player = GetComponent<Player>();
    if (player != null)
    {
        player.SetInvincible(true);
    }

    float timer = 0;

    dodgeDirection = (transform.forward * directionInput.y + transform.right * directionInput.x).normalized;

    if (directionInput.y > 0)
    {
        anim.SetTrigger("DashF");
    }
    else if (directionInput.y < 0)
    {
        anim.SetTrigger("DashB");
    }
    else if (directionInput.x > 0) 
    {
        anim.SetTrigger("DashR");
    }
    else if (directionInput.x < 0) 
    {
        anim.SetTrigger("DashL");
    }

    Vector3 targetPosition = transform.position + dodgeDirection * distance;

    while (timer < dodgeTimer)
    {
        float speed = dodgeCurve.Evaluate(timer);
        Vector3 dir = (dodgeDirection * speed) + (Vector3.up * playerController.velocity.y);

        characterController.Move(dir * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, timer / dodgeTimer); 

        timer += Time.deltaTime;
        yield return null;
    }

    if (player != null)
    {
        player.SetInvincible(false);
    }

    isDodging = false;
    playerController.isDodging = false;  
}

}
