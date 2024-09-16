using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHeadLook : MonoBehaviour
{
    public Transform character; 
    public Transform head; 
    public float rotationSpeed = 5f; 

    // Make the npc turn their head to track the player position
    void Update()
    {
        if (character != null && head != null)
        {
            Vector3 direction = character.position - head.position;
            direction.y = 0; 
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            head.rotation = Quaternion.Slerp(head.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
