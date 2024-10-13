using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [SerializeField] AnimationCurve dodgeCurve;
    PlayerController playerController;  
    Animator anim;
    CharacterController characterController;
    bool isDodging = false;
    float dodgeTimer;
    Vector3 dodgeDirection;

    void Start()
    {
        playerController = GetComponent<PlayerController>(); 
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();

        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.keys.Length - 1];
        dodgeTimer = dodge_lastFrame.time;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDodging && playerController.movement.magnitude > 0)
        {
            StartCoroutine(Dodging());
        }
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
}
