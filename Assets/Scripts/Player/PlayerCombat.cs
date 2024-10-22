using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    
    private Animator anim;
    private Transform currentTarget;
    private CharacterController controller;
    private PlayerTargetLock targetLock;
    private PlayerController playerController;
    private Dodge dodge; 
    
    private float speedSmoothVelocity;
    private float speedSmoothTime;
    public float currentSpeed;
    public bool isInCombat;
    private Vector3 moveInput;
    private Vector3 dir;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        targetLock = GetComponent<PlayerTargetLock>();
        dodge = GetComponent<Dodge>(); 
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        if (targetLock.isTargeting)
        {
            currentTarget = targetLock.currentTarget;
            EngageCombatMode();
        }
        else
        {
            DisengageCombatMode();
        }
    }

    private void GetInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (dodge != null && !dodge.isDodging && targetLock.isTargeting)
        {
            dodge.SetDodgeDirection(moveInput);
        }

        if (targetLock.isTargeting && currentTarget != null)
        {
            Vector3 toTarget = (currentTarget.position - transform.position).normalized;

            Vector3 forward = new Vector3(toTarget.x, 0, toTarget.z).normalized;
            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            dir = (forward * moveInput.y + right * moveInput.x).normalized;
        }
    }

    private void EngageCombatMode()
    {
        playerController.isInCombat = true;
        isInCombat = true;
        anim.SetLayerWeight(anim.GetLayerIndex("CombatLayer"), 1);      
        anim.SetBool("IsInCombat", true); 

        if(!dodge.isDodging)
        {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, moveSpeed, ref speedSmoothVelocity, speedSmoothTime * Time.deltaTime);
        
        Vector3 velocity = dir * currentSpeed;
        controller.Move(velocity * Time.deltaTime); 
        }
        
        anim.SetFloat("Speed", dir.magnitude, 0.1f, Time.deltaTime);
        anim.SetFloat("Horizontal", moveInput.x, 0.1f, Time.deltaTime);
        anim.SetFloat("Vertical", moveInput.y, 0.1f, Time.deltaTime);
    }

    private void DisengageCombatMode()
    {
        anim.SetBool("IsInCombat", false); 
        StartCoroutine(ResetCombatLayer()); 
    }

    private IEnumerator ResetCombatLayer()
    {
        yield return new WaitForSeconds(0.15f);
        playerController.isInCombat = false;
        isInCombat = false;
        anim.SetLayerWeight(anim.GetLayerIndex("CombatLayer"), 0);
    }
}
