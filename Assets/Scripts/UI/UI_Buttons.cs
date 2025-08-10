using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Buttons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    private UI_Animator uiAnimator;
    private RectTransform myRect;

    [SerializeField] private float showcaseScale = 1.1f;
    [SerializeField] private float scaleUpDuration = 0.2f;

    private Coroutine scaleCoroutine;
    [Space(10)]
    [SerializeField] private UI_TextBlinkEffect myTextBlinkEffect;
    private void Awake()
    {
        uiAnimator = GetComponentInParent<UI_Animator>();
        myRect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }

        scaleCoroutine = StartCoroutine(uiAnimator.ChangeScaleCorotine(myRect, showcaseScale, scaleUpDuration));
        if(myTextBlinkEffect != null)
        {
            myTextBlinkEffect.EnableBlink(false);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        } 
        scaleCoroutine = StartCoroutine(uiAnimator.ChangeScaleCorotine(myRect, 1f, scaleUpDuration));
        if (myTextBlinkEffect != null)
        {
            myTextBlinkEffect.EnableBlink(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        myRect.localScale = new Vector3(1,1,1);
    }
}
