using UnityEngine;
using UnityEngine.AI;

public class Minotaur : BaseEnemy
{
    // Implement the DropLoot method specific to Minotaur
    protected override void DropLoot()
    {
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
            }
        }
        Debug.Log("Minotaur loot dropped");
    }
}
