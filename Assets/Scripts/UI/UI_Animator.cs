using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Animator : MonoBehaviour
{
    [Header("UI Feedback")]
    [SerializeField] private float shakeManitude;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeRotationMagnitude;
    [Space(10)]
    [SerializeField] private float defaultUiScale = 1.5f;
    [SerializeField] private bool scaleChangeAvailable ; 
    public void Shake(Transform transformToShake)
    {
        RectTransform rectTransform = transformToShake.GetComponent<RectTransform>();
        StartCoroutine(ShakeCoroutine(rectTransform));
    }
    private IEnumerator ShakeCoroutine(RectTransform rectTransform)
    {
        float time = 0f;
        Vector3 originalPosition = rectTransform.anchoredPosition;
        float currentScale = rectTransform.localScale.x;

        if (scaleChangeAvailable)
            StartCoroutine(ChangeScaleCorotine(rectTransform, currentScale*1.1f,shakeDuration/2));

        while (time < shakeDuration)
        {
            float xOffset = Random.Range(-shakeManitude, shakeManitude);
            float yOffset = Random.Range(-shakeManitude, shakeManitude);
            float randomRotation = Random.Range(-shakeRotationMagnitude, shakeRotationMagnitude);
            rectTransform.anchoredPosition = originalPosition + new Vector3(xOffset, yOffset, 0);
            rectTransform.localRotation = Quaternion.Euler(0, 0, randomRotation);
            time += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        rectTransform.anchoredPosition = originalPosition; // Reset to original position
        rectTransform.localRotation = Quaternion.Euler(Vector3.zero); // Reset rotation

        if(scaleChangeAvailable)
        {
            StartCoroutine(ChangeScaleCorotine(rectTransform, defaultUiScale, shakeDuration / 2));
        }
    }




    public void ChangePosition(Transform transform, Vector3 offset, float duration = .1f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        StartCoroutine(ChangePositionCorotine(rectTransform, offset, duration));
    }
    private IEnumerator ChangePositionCorotine(RectTransform rectTransform, Vector3 offset, float duration)
    {
        float time = 0f;

        Vector3 initPosition = rectTransform.anchoredPosition;
        Vector3 targetPosition = initPosition + offset;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(initPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        rectTransform.anchoredPosition = targetPosition; // Ensure final position is set
    }

    public void ChangeScale(Transform transform, float targetScale, float duration = .25f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        StartCoroutine(ChangeScaleCorotine(rectTransform, targetScale, duration));
    }

    public IEnumerator ChangeScaleCorotine(RectTransform rectTransform, float newScale, float duration = .25f)
    {
        float time = 0f;
        Vector3 initScale = rectTransform.localScale;
        Vector3 targetScale = new Vector3(newScale, newScale, newScale);
        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(initScale, targetScale, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null; // Wait for the next frame
        }
        rectTransform.localScale = targetScale; // Ensure final scale is sets
    }

    public void ChangeColor(Image image, float targetAlpha, float duration)
    {
        StartCoroutine(ChangeColorCoroutine(image, targetAlpha, duration));
    }

    private IEnumerator ChangeColorCoroutine(Image image, float targetAlpha, float duration)
    {
        float time = 0f;
        Color currentColor = image.color;
        float startAlpha = currentColor.a;
        while (time < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            time += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha); // Ensure final color is set
    }
}
