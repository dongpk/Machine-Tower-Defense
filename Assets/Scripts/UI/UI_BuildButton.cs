using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BuildButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UI ui;
    private BuildManager buildManager;
    private CameraEffect cameraEffect;
    private GameManager gameManager;
    private TowerAttackRadiusDisplay towerAttackRadiusDisplay;
    private UI_BuildButtonsHolder buildButtonsHolder;
    private UI_BuildButtonOnHoverEffect onHoverEffect;


    [SerializeField] private string towerName;
    [SerializeField] private int towerPrice = 50;

    [Space(10)]
    [SerializeField] private GameObject towerToBuild;
    [SerializeField] private float towerCenterY = 0.5f;
    [Header("Text Components")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField]private TextMeshProUGUI towerPriceText;
    

    //xem trc thap trc khi xay
    private TowerPreview towerPreview;
    public bool buttonUnlocked { get; private set; } 

    private void Awake()
    {
        ui  = GetComponentInParent<UI>();
        buildButtonsHolder = GetComponentInParent<UI_BuildButtonsHolder>();
        onHoverEffect = GetComponent<UI_BuildButtonOnHoverEffect>();

        cameraEffect = FindFirstObjectByType<CameraEffect>();
        buildManager = FindFirstObjectByType<BuildManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        towerAttackRadiusDisplay = FindFirstObjectByType<TowerAttackRadiusDisplay>(FindObjectsInactive.Include);


    }
    private void Start()
    {
        CreateTowerPreview(); 
    }
    private void CreateTowerPreview()
    {
        // Thêm kiểm tra null
        if (towerToBuild == null)
        {
            Debug.LogError($"Tower prefab is null for button {towerName}!");
            return;
        }
        GameObject newPreview = Instantiate(towerToBuild, Vector3.zero, Quaternion.identity);
        towerPreview = newPreview.AddComponent<TowerPreview>();

        towerPreview.gameObject.SetActive(false);
    }
    public void SelectButton(bool select)
    {
        // Kiểm tra towerPreview
        if (towerPreview == null)
        {
            Debug.LogWarning($"Tower preview is null for {towerName}! Creating now...");
            CreateTowerPreview();
            // Kiểm tra lại sau khi tạo
            if (towerPreview == null)
                return;
        }
        BuildSlot slotToUse = buildManager.GetSelectedSlot();
        if(slotToUse==null)
            return;
        Vector3 previewPosition = slotToUse.GetBuildPosition(1);

        towerPreview.gameObject.SetActive(select);
        towerPreview.ShowPreview(select, previewPosition);

        onHoverEffect.ShowcaseButton(select);
        buildButtonsHolder.SetLastSelected(this);
    }
    public void UnlockTowerIfNeeded(string towerNametTocCheck, bool unlockStatus)
    {
        if(towerNametTocCheck!= towerName)
            return;

        buttonUnlocked = unlockStatus;
        gameObject.SetActive(unlockStatus);
    }

    public void BuildTower()
    {

        if (gameManager.HasEnoughCurrency(towerPrice) == false)
        {
            Debug.LogWarning("Đủ tiền đéo đâu mà xây tháp");
            ui.ingameUI.ShakeCurrencyUI();
            return;
        }

        if (towerToBuild == null)
        {
            Debug.LogWarning("Gán tháp vào đê dcm");
            return;
        }

        if(ui.buildButtonsUI.GetLastSelectedButton() == null)
        {
            return;
        }
        BuildSlot slotToUse = buildManager.GetSelectedSlot();
        buildManager.CancelBuildAction();
        slotToUse.SnapToDefaultPositionImmidiately();
        slotToUse.SetSlotAvailableTo(false);

        ui.buildButtonsUI.SetLastSelected(null);

        cameraEffect.ScreenShake(0.2f, 0.03f);

        GameObject newTower = Instantiate(towerToBuild, slotToUse.GetBuildPosition(towerCenterY), Quaternion.identity);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buildManager.MouseOverUI(true);
        foreach (var button in buildButtonsHolder.GetBuildButtons())
        {
            button.SelectButton(false);
        }
        SelectButton(true);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buildManager.MouseOverUI(false);
    }
    private void OnValidate()
    {
        towerNameText.text = towerName;
        towerPriceText.text = towerPrice.ToString();
        gameObject.name = " BuildButton_UI - " + towerName;
    }

}
