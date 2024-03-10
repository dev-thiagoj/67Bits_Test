using TMPro;
using UnityEngine;

public class IconsController : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] TextMeshProUGUI walletDisplay;
    [SerializeField] TextMeshProUGUI bodiesDisplay;

    private void Start()
    {
        var displays = GetComponentsInChildren<TextMeshProUGUI>();

        walletDisplay = displays[0];
        bodiesDisplay = displays[1];
    }

    public void OnValueChanged(int walletAmount, int bodiesAmount)
    {
        if (!walletDisplay)
            return;

        walletDisplay.text = walletAmount.ToString();
        bodiesDisplay.text = bodiesAmount.ToString();
    }

    //public void ShowStore(bool value)
    //{
    //    storeBtn.gameObject.SetActive(value);
    //}
}
