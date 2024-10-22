using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShopkeeperInteract : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private bool haveInteracted = false;

    public ShopkeeperUI shopkeeperUI;

    //player clicks on npc
    public void OnClick(NavMeshAgent playerAgent)
    {
        this.playerAgent = playerAgent;
        playerAgent.stoppingDistance = 2;
        playerAgent.SetDestination(transform.position);
        haveInteracted = false;
    }

    // palyer within range of npc and can interact
    void Update()
    {
        if (playerAgent != null && !playerAgent.pathPending && !haveInteracted)
        {
            if (playerAgent.remainingDistance <= 2)
            {
                Interact();
                haveInteracted = true;
            }
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("Player interacted with shopkeeper");
        OpenShop();
    }

    private void OpenShop()
    {
        if (shopkeeperUI != null)
        {
            shopkeeperUI.Show();
        }
        else
        {
            Debug.LogWarning("ShopkeeperUI not assigned");
        }
    }
}
