using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    private CameraController cameraController;
    [Header("Keyboard Sensitivity")]
    [SerializeField] private Slider keyboardSensitivitySlider;
    [SerializeField] private TextMeshProUGUI keyboardSensitivityText;
    [SerializeField] private string keyboardSensParameter = "KeyboardSensitivity";

    [SerializeField] private float minKeyboardSensitivity = 60;
    [SerializeField] private float maxKeyboardSensitivity = 240;

    [Header("Mouse Sensitivity")]
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private TextMeshProUGUI mouseSensitivityText;
    [SerializeField] private string mouseSensParameter = "MouseSensitivity";

    [SerializeField] private float minMouseSensitivity = 1;
    [SerializeField] private float maxMouseSensitivity = 10;


    private void Awake()
    {
        cameraController = FindFirstObjectByType<CameraController>();
    }

    public void KeyboardSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minKeyboardSensitivity, maxKeyboardSensitivity, value);
        cameraController.AdjustKeyboardSensitivity(newSensitivity);

        keyboardSensitivityText.text = Mathf.RoundToInt(value * 100) + "%";
    }
    public void MouseSensitivity(float value)
    {
        float newSensitivity = Mathf.Lerp(minMouseSensitivity, maxMouseSensitivity, value);
        cameraController.AdjustMouseSensitivity(newSensitivity);

        mouseSensitivityText.text = Mathf.RoundToInt(value * 100) + "%";
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(keyboardSensParameter, keyboardSensitivitySlider.value);
        PlayerPrefs.SetFloat(mouseSensParameter, mouseSensitivitySlider.value);
    }
    private void OnEnable()
    {
        keyboardSensitivitySlider.value = PlayerPrefs.GetFloat(keyboardSensParameter, .5f);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat(mouseSensParameter, .5f);
    }
}
