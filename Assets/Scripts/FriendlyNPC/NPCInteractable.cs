using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        // animator = GetComponent<animator>();

    }
    public void Interact()
    {
        Debug.Log("Interact");
        animator.SetTrigger("Talk");
    }
}
