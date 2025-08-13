using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    private UI ui;
    [SerializeField] private UI_Pause pauseUI;
    private UI_Animator uiAnimator;


    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI healPointText;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private float waveTimerOffset;
    [SerializeField] UI_TextBlinkEffect waveTimerBlinkEffect;


    private void Awake()
    {
        uiAnimator = GetComponentInParent<UI_Animator>();
        ui = GetComponentInParent<UI>();

    }
    private void Update()
    {
        // Kiểm tra null trước khi sử dụng
        if (Input.GetKeyDown(KeyCode.F10))
        {
            ui.SwitchTo(pauseUI.gameObject);
        }
    }
    public void ShakeCurrencyUI() => ui.uiAnimator.Shake(currencyText.transform.parent);
    public void ShakeHealPointUI() => ui.uiAnimator.Shake(healPointText.transform.parent);

    public void UpdateHealPointUI(int value, int maxValue)
    {
        int newValue = maxValue - value;
        healPointText.text = "thread : " + newValue + "/" + maxValue;
    }
    public void UpdateCurrencyUI(int value)
    {
        currencyText.text = "Gold : " + value;
    }

    public void UpdateWaveTimerUI(float value) => waveTimerText.text = "seconds : " + value.ToString("00");
    public void EnableWaveTimer(bool enable)
    {
        // Kiểm tra null để tránh lỗi
        if (waveTimerText == null || uiAnimator == null)
        {
            Debug.LogWarning("Wave timer text or UI Animator is missing!");
            return;
        }
        Transform waveTimerTransform = waveTimerText.transform.parent;
        float yOffset = enable ? -waveTimerOffset : waveTimerOffset;
        Vector3 offset = new Vector3(0, yOffset);

        uiAnimator.ChangePosition(waveTimerTransform, offset);
        waveTimerBlinkEffect.EnableBlink(enable);

        //waveTimerText.transform.parent.gameObject.SetActive(enable);
    }

    public void ForceWaveButton()
    {
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        waveManager.ForceNextWave();
    }


}
