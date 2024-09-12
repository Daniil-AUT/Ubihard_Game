using UnityEngine;

public class Skeleton : BaseEnemy
{
    protected override void DropLoot()
    {
        Debug.Log("Entering DropLoot method for Skeleton"); // Debug log to check if method is called

        int count = Random.Range(1, 4); // Number of items to drop
        for (int i = 0; i < count; i++)
        {
            ItemSO item = ItemDBManager.Instance.GetRandomItem();
            if (item != null && item.prefab != null)
            {
                GameObject go = Instantiate(item.prefab, transform.position, Quaternion.identity);
                go.tag = "Interactable";
                PickableObject po = go.AddComponent<PickableObject>();
                po.itemSO = item;

                Debug.Log($"Dropped item: {item.name}"); // Debug log to check which items are dropped
            }
            else
            {
                Debug.LogWarning("Item is null or prefab is missing."); // Warning for missing items
            }
        }
        Debug.Log("Skeleton loot dropped"); // Confirm loot drop completion
    }
}
