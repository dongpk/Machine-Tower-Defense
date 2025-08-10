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
        currentHP= maxHP;
        inGameUI.UpdateHealPointUI(currentHP, maxHP);
    }

    public void UpdateHP(int value)
    {
        currentHP+= value;
         inGameUI.UpdateHealPointUI(currentHP, maxHP);
    }
    public void UpdateCurrency(int value)
    {
        currency += value;
        inGameUI.UpdateCurrencyUI(currency);
    }
}
