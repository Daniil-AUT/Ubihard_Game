using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopObject : ShopkeeperInteract
{

    private bool playerInShopRange = false;

    //player enters the shop's interaction range
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Player entered the shop's range");
            playerInShopRange = true;
            //show the shopkeeper UI
            ShopkeeperUI.Instance.Show();
        }
    }

    //player exits the shop's interaction range
    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Player exited the shop's range");
            playerInShopRange = false;
            //hide the shopkeeper UI
            ShopkeeperUI.Instance.Hide();
        }
    }

    // interact method for when player is in range
    protected override void Interact()
    {
        if (playerInShopRange)
        {
            Debug.Log("Player interacting with shopkeeper");
            ShopkeeperUI.Instance.Show();
        }
    }
}
