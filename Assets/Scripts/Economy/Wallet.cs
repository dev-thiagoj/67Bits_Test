using System;

public class Wallet
{
    PlayerStats _stats;
    int _amount;
    
    public Wallet(PlayerStats stats) => Reset(stats);

    public int Amount => _amount;

    public void Reset(PlayerStats stats)
    {
        _stats = stats;
        _amount = 0;
    }

    public void Add(int value)
    {
        _amount += value;
        _stats.OnValueChanged?.Invoke(_amount, _stats.TotalBodies);
    }

    public void Remove(int value)
    {
        if (_amount < value)
            return;

        _amount -= value;
        _stats.OnValueChanged?.Invoke(_amount, _stats.TotalBodies);
    }
}
