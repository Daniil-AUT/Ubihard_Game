using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerCombat playerCombat;
    private Enemy enemy;
    private FinalBoss boss; 
    private Animator anim;

    public bool isCrouching = false;
    public float crouchSpeed = 2f; 

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerCombat = GetComponent<PlayerCombat>();        
        anim = GetComponentInChildren<Animator>();
        GameObject minotaur = GameObject.Find("Enemies/minotaur1");
        GameObject bossObject = GameObject.Find("FinalBoss");
        enemy = minotaur.GetComponent<Enemy>();
        boss = bossObject.GetComponent<FinalBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !playerCombat.isInCombat) 
        {
            ToggleCrouch();
        }
    }

    void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        anim.SetBool("IsCrouching", isCrouching); 

        if (isCrouching)
        {
            enemy.detectionRange = 5f;
            boss.detectionRange = 5f;
            playerController.currentSpeed = crouchSpeed; 
        }
        else
        {
            enemy.detectionRange = 20f;
            boss.detectionRange = 20f;
            playerController.currentSpeed = playerController.walkSpeed; 
        }
    }
}
