using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int currency;

    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;

    private UI_InGame inGameUI;


    private void Awake()
    {
        inGameUI = FindFirstObjectByType<UI_InGame>(FindObjectsInactive.Include);
    }
    private void Start()
    {
        currentHP = maxHP;
        inGameUI.UpdateHealPointUI(currentHP, maxHP);
        inGameUI.UpdateCurrencyUI(currency);
    }


    public void UpdateHP(int value)
    {
        currentHP += value;
        inGameUI.UpdateHealPointUI(currentHP, maxHP);
        inGameUI.ShakeHealPointUI();
    }
    public void UpdateCurrency(int value)
    {
        currency += value;
        inGameUI.UpdateCurrencyUI(currency);

    }
    public bool HasEnoughCurrency(int price)
    {

        Debug.Log($"spend {price}");
        if (price <= currency)
        {
            currency = currency - price;
            inGameUI.UpdateCurrencyUI(currency);
            return true;
        }
        return false;
    }
}
