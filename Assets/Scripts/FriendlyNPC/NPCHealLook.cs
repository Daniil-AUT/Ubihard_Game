using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHeadLook : MonoBehaviour
{
    public Transform character; // Assign the target character in the Inspector
    public Transform head; // Assign the NPC's head in the Inspector
    public float rotationSpeed = 5f; // Speed at which the head turns

    void Update()
    {
        if (character != null && head != null)
        {
            // Calculate the direction from the head to the character
            Vector3 direction = character.position - head.position;
            direction.y = 0; // Ignore vertical differences (e.g., for a 2D effect)

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate the head towards the target rotation
            head.rotation = Quaternion.Slerp(head.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}