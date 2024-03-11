using UnityEngine;
using UnityEngine.Events;

public class Store : MonoBehaviour
{
    PlayerStats _stats;

    public UnityEvent<Store, PlayerStats> OnStoreEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!other.TryGetComponent(out PlayerStats stats))
            return;

        stats.CanMove = false;

        if (!_stats)
            _stats = stats;

        OnStoreEntered?.Invoke(this, stats);
    }

    public void Buy()
    {
        _stats.LevelController.LevelUp();
    }
        
}
