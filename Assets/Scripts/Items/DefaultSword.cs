using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSword : MonoBehaviour
{
    [SerializeField] float weaponDamage; 
    private bool canDealDamage;

    void Start()
    {
        canDealDamage = false;
    }

    public void EnableDamage()
    {
        canDealDamage = true; 
        StartCoroutine(DisableDamageAfterDelay(1.5f)); 
    }

    private IEnumerator DisableDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canDealDamage = false; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (canDealDamage && other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit (" + weaponDamage + " dmg)");
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)weaponDamage);
            }
        }
    }
}
