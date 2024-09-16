using UnityEngine;

public class Skeleton : Enemy
{
    protected override void DropLoot()
    {
        Debug.Log("can the enemy drop loot???");

        // Drop a random number of items (should be different in type)
        int count = Random.Range(1, 4); 
        for (int i = 0; i < count; i++)
        {
            ItemSO item = ItemDBManager.Instance.GetRandomItem();
            if (item != null && item.prefab != null)
            {
                GameObject go = Instantiate(item.prefab, transform.position, Quaternion.identity);
                go.tag = "Interactable";
                PickableObject po = go.AddComponent<PickableObject>();
                po.itemSO = item;
                // check if itemw was dropped
                Debug.Log("item dropped");
            }
            else
            {
                // check if the items exits
                Debug.LogWarning("no item"); 
            }
        }
    }
}
