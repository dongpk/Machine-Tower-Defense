using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BuildButtonOnHoverEffect : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float ajustmentSpeed = 10;
    [SerializeField] float showCaseY;
    [SerializeField] private float defaultY;


    private float targetY;
    private bool canMove;
    private void Update()
    {
        if(Mathf.Abs(transform.position.y - targetY) > 0.01f&& canMove)
        {
            float newPositionY = Mathf.Lerp(transform.position.y, targetY, ajustmentSpeed * Time.deltaTime);

            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }
    }
    public void ToggleMovement(bool buttonMenuActive)
    {
        canMove = buttonMenuActive;
        SetTargetY(defaultY);
        if (buttonMenuActive == false)
        {
            SetPositionToDefault();
        }
    }

    private void SetPositionToDefault()
    {
        transform.position = new Vector3(transform.position.x, defaultY, transform.position.z);
    }

    private void SetTargetY(float newT)=> targetY = newT;

    public void OnPointerEnter(PointerEventData eventData) => SetTargetY(showCaseY);       
    public void OnPointerExit(PointerEventData eventData) => SetTargetY(defaultY);
}
