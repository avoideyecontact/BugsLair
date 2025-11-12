using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    private List<ItemDropType> drop;

    private void Start()
    {
        drop = new List<ItemDropType>();
    }

    public void Add(ItemDropType type)
    {
        drop.Add(type);
    }

    public void Remove(ItemDropType type)
    {
        drop.Remove(type);
    }
}
