using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    private UI ui;
    private UI_InGame inGameUI;

    [SerializeField] private GameObject[] pauseUiElements;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        inGameUI = GetComponentInChildren<UI_InGame>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ui.SwitchTo(inGameUI.gameObject);
        }
    }

    public void SwitchPauseUiElements(GameObject uiElementsToEnable)
    {
        foreach (GameObject obj in pauseUiElements)
        {
            obj.SetActive(false);
        }
        uiElementsToEnable.SetActive(true);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f; // Dừng thời gian khi UI Pause được bật
    }
    private void OnDisable()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian khi UI Pause bị tắt
    }
}
