using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI healPointText;

    public void UpdateHealPointUI(int value,int maxValue)
    {
        int newValue = maxValue - value;
        healPointText.text = "thread : "+ newValue + "/"+ maxValue;
    }
    public void UpdateCurrencyUI(int value)
    {
        currencyText.text = "Gold : " + value;
    }
}
