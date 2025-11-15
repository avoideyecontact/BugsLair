using UnityEngine;
using VContainer.Unity;

public class Statistics : IStatistics, IInitializable
{
    private IEventBus _eventBus;
    public static StatisticsData _data;

    public static int _cockroachesKilled;
    public static int _spidersKilled;

    public StatisticsData Data => _data;

    public Statistics(IEventBus eventBus)
    {
        _eventBus = eventBus;
        SubscribeToEventBus();
    }

    public void Initialize()
    {
        _data = new StatisticsData();
        _cockroachesKilled = 0;
        _spidersKilled = 0;
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<CockroachKilled>(OnCockroachKill);
        _eventBus.Subscribe<PlayerDamaged>(OnPlayerDamage);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<CockroachKilled>(OnCockroachKill);
        _eventBus.Unsubscribe<PlayerDamaged>(OnPlayerDamage);
    }

    private void OnCockroachKill(CockroachKilled evt)
    {
        _data.CockroachesKilled += 1;
        Debug.Log(_data.CockroachesKilled);
    }

    private void OnPlayerDamage(PlayerDamaged evt)
    {
        _data.DamageTaken += evt.DamageValue;
    }

    ~Statistics()
    {
        UnsubscribeFromEventBus();
    }
}
