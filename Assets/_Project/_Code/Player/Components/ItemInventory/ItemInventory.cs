using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class ItemInventory : MonoBehaviour
{
    private List<ItemDropType> drop;
    private IEventBus _eventBus;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    private void Start()
    {
        drop = new List<ItemDropType>();
    }

    public void Add(ItemDropType type)
    {
        drop.Add(type);

        _eventBus.Publish(new ItemInventoryChanged
        {
            items = drop
        });
    }

    public void Remove(ItemDropType type)
    {
        drop.Remove(type);

        _eventBus.Publish(new ItemInventoryChanged
        {
            items = drop
        });
    }

    public bool Has(ItemDropType item)
    {
        return drop.Where(i => i == item).Count() > 0;
    }
}
