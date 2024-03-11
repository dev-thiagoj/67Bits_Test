using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    int totalBodiesSold;
    bool _canMove;

    public LevelController LevelController { get; private set; }
    public Wallet Wallet { get; private set; }
    public int TotalBodies => totalBodiesSold;

    [Space]
    public UnityEvent<int, int> OnValueChanged;
    public bool CanMove
    {
        get => _canMove;
        set => _canMove = value;
    }

    private void Start()
    {
        Wallet = new(this);

        _canMove = true;


        if (LevelController)
            return;

        LevelController = GetComponent<LevelController>();
        LevelController.OnLevelUpped.AddListener(OnBuyLevelUp);
    }

    public void OnSellBodies(int value)
    {
        totalBodiesSold++;
        Wallet.Add(value);
    }

    void OnBuyLevelUp(int index)
    {
        var cost = LevelController.Setup.GetSetupByLevel((PlayerLevel)index).cost;
        Wallet.Remove(cost);
        _canMove = true;
    }

#if UNITY_EDITOR
    public int debugValue;
    [ContextMenu("Debug Money")]
    void AddMoney()
    {
        Wallet.Add(debugValue);
    }
#endif
}
