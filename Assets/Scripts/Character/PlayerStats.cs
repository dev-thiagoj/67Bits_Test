using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    int totalBodiesSold;

    public bool CanMove {  get; private set; }
    public Wallet Wallet { get; private set; }

    public int TotalBodies => totalBodiesSold;

    [Space]
    public UnityEvent<int, int> OnValueChanged;

    private void Start()
    {
        Wallet = new(this);
    }

    public void OnSell(int value)
    {
        totalBodiesSold++;
        Wallet.Add(value);
    }
}
