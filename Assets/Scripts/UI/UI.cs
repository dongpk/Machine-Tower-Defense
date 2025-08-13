using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Image fadeImageUI;
    [SerializeField] private GameObject[] uiElements;

    private UI_Settings settingsUI;
    private UI_MainMenu mainMenuUI;

    public UI_InGame ingameUI { get; private set; }
    public UI_Animator uiAnimator { get; private set; }
    public UI_BuildButtonsHolder buildButtonsUI { get; private set; }
    private void Awake()
    {
        buildButtonsUI = GetComponentInChildren<UI_BuildButtonsHolder>(true);
        settingsUI = GetComponentInChildren<UI_Settings>(true);
        mainMenuUI = GetComponentInChildren<UI_MainMenu>(true);
        ingameUI = GetComponentInChildren<UI_InGame>(true);
        uiAnimator = GetComponent<UI_Animator>();

        //ActivateFadeEffect(true); 

        SwitchTo(settingsUI.gameObject);
        //SwitchTo(mainMenuUI.gameObject);
        SwitchTo(ingameUI.gameObject);
    }
    public void SwitchTo(GameObject uiToEnable)
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        uiToEnable.SetActive(true);
    }
    public void QuitButton()
    {
        if (Application.isPlaying)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
    public void ActivateFadeEffect(bool fadeIn)
    {
        if (fadeIn)
        {
            uiAnimator.ChangeColor(fadeImageUI, 0f, 2f);
        }
        else
        {
            uiAnimator.ChangeColor(fadeImageUI, 1f, 2f);
        }
    }
}
