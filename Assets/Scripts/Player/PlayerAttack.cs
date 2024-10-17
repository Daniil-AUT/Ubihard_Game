using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Player controller
    private PlayerController playerController;
    private Animator anim;
    private DefaultSword sword;
    private Dodge dodge;
    private PlayerCrouch playerCrouch;

    private float attackCooldown = 1f; 
    private float lastAttackTime = 0f; 
    private bool isComboActive = false; 
    private bool canPerformSecondAttack = false; 
    public bool isAttacking = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        sword = GetComponentInChildren<DefaultSword>();
        dodge = GetComponent<Dodge>();
        playerCrouch = GetComponent<PlayerCrouch>();
    }

    void Update()
    {
        if (!playerController.isDodging && !playerController.isJumping && !playerController.sprinting && !playerCrouch.isCrouching)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                if (!isComboActive) 
                {
                    PerformAttack1(); 
                }
                else if (canPerformSecondAttack) 
                {
                    PerformAttack2(); 
                }
            }
        }
    }

    private void PerformAttack1()
    {
        isComboActive = true; 
        playerController.isAttacking = true; 
        isAttacking = true;
        canPerformSecondAttack = true; 
        anim.SetTrigger("Attack1");
        lastAttackTime = Time.time;
        sword.EnableDamage();
        StartCoroutine(ResetComboState(attackCooldown)); 
    }

    private void PerformAttack2()
    {
        canPerformSecondAttack = false; 
        anim.SetTrigger("Attack2");
        sword.EnableDamage();
        StartCoroutine(ResetAttackState(1.5f)); 
    }

    private IEnumerator ResetComboState(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (canPerformSecondAttack) 
        {
            ResetAttack(); 
        }
    }

    private void ResetAttack()
    {
        canPerformSecondAttack = false; 
        isComboActive = false; 
        playerController.isAttacking = false; 
        isAttacking = false;
    }

    private IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay); 
        ResetAttack(); 
    }
}
