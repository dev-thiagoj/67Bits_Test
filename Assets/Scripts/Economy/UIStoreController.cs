using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStoreController : MonoBehaviour
{
    PlayerStats _stats;
    [SerializeField] Store _store;

    [SerializeField] TextMeshProUGUI priceDisplay;
    [SerializeField] Button cancelButton;
    [SerializeField] Button buyButton;

    GameObject child;
    string message;

    float lastPressed;
    float threshold = .2f;


    private void Start()
    {
        child = transform.GetChild(0).gameObject;
        Hide();
    }

    public void Open(Store store, PlayerStats stats)
    {
        if (!_store)
            _store = store;

        if(!_stats)
            _stats = stats;

        cancelButton.onClick.AddListener(Hide);
        buyButton.onClick.AddListener(Confirm);

        Show();
    }

    void Show()
    {
        child.SetActive(true);

        buyButton.interactable = IsValid();

        if(!buyButton.interactable)
            priceDisplay.color = Color.red;

        priceDisplay.text = message;
    }

    void Hide()
    {
        child.SetActive(false);

        if (!_stats)
            return;

        _stats.CanMove = true;
    }

    void Confirm()
    {
        if (lastPressed >= Time.time)
            return;

        lastPressed = Time.time + threshold;

        _store.Buy();
        Hide();
    }

    bool IsValid()
    {
        message = null;
        priceDisplay.color = Color.white;

        bool canGoUp = _stats.LevelController.CurrentLevel != PlayerLevel.SerialKiller;

        if (!canGoUp)
        {
            message = "Max level reached.";
            return false;
        }

        int nextLevel = (int)_stats.LevelController.CurrentSetup.level + 1;
        int price = _stats.LevelController.Setup.GetSetupByLevel((PlayerLevel)nextLevel).cost;
        int amount = _stats.Wallet.Amount;
        bool canBuy = amount >= price;

        if (!canBuy)
        {
            message = "Without enough money";
            return false;
        }

        message = $"${price}";
        return true;
    }
}
