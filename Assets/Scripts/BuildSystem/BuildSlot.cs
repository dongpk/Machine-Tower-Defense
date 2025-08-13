using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private TileAnimator tileAnimator;
    private BuildManager buildManager;
    private Vector3 defaultPosition;

    private bool tileCanBeMove = true;
    private bool buildSlotAvailable = true;

    private Coroutine currentMovementUpCoroutine;
    private Coroutine moveToDefaultCoroutine;

    private void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        tileAnimator = FindFirstObjectByType<TileAnimator>();
        buildManager = FindFirstObjectByType<BuildManager>();
        defaultPosition = transform.position;
    }
    private void Start()
    {
        if (buildSlotAvailable == false)
        {
            transform.position += new Vector3(0, 0.1f);
        }
    }

    public void SetSlotAvailableTo(bool value) => buildSlotAvailable = value;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buildSlotAvailable == false)
            return;

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
            return;

        if (tileCanBeMove == false)
            return;
        MoveTileUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buildSlotAvailable == false)       
            return;
        

        if (tileCanBeMove == false)
            return;

        if (currentMovementUpCoroutine != null)
        {
            Invoke(nameof(MoveToDefaultPosition), tileAnimator.GetTravelDuration());
        }
        else
            MoveToDefaultPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buildSlotAvailable == false)
        {
            return;
        }
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (buildManager.GetSelectedSlot() == this)
        {
            return; // If this slot is already selected, do nothing
        }
        buildManager.EnableBuildMenu();
        buildManager.SelectBuildSlot(this);
        MoveTileUp();

        tileCanBeMove = false;

        //ui.buildButtonsUI.GetLastSelectedButton()?.SelectButton(true);
        // Kiểm tra null để tránh lỗi
        if (ui != null && ui.buildButtonsUI != null)
        {
            ui.buildButtonsUI.GetLastSelectedButton()?.SelectButton(true);
        }
        else
        {
            Debug.LogWarning("UI or buildButtonsUI is null. Check references!");
        }
    }
    public void UnselectTile()
    {
        MoveToDefaultPosition();
        tileCanBeMove = true;
    }

    private void MoveTileUp()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, tileAnimator.GetBuildSlotOffset(), 0);
        currentMovementUpCoroutine = StartCoroutine(tileAnimator.MoveTileCoroutine(transform, targetPosition));


    }
    private void MoveToDefaultPosition()
    {
        moveToDefaultCoroutine = StartCoroutine(tileAnimator.MoveTileCoroutine(transform, defaultPosition));
    }
    public void SnapToDefaultPositionImmidiately()
    {
        if (moveToDefaultCoroutine != null)
        {
            StopCoroutine(moveToDefaultCoroutine);
        }
        transform.position = defaultPosition;
    }
    public Vector3 GetBuildPosition(float yOffset) => defaultPosition + new Vector3(0, yOffset);


}
