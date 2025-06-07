using UnityEngine;
using UnityEngine.UI;

public class ToolMenu : MonoBehaviour
{
    [SerializeField] private ToolManager toolManager;
    [SerializeField] private StatusMessage statusMessageUI;
    [SerializeField] private Valve targetValve;
    [SerializeField] private Lid targetLid;
    [SerializeField] private Bolt[] targetBolt;

    [Header("UI Elements")]
    [SerializeField] private GameObject valveButton;
    [SerializeField] private Button boltButton;
    [SerializeField] private Text boltCountText; 
    [SerializeField] private GameObject lidButton;

    private bool hasValve = true;
    private bool hasLid = false;
    private int boltCount = 0;

    private void Awake()
    {
        UpdateInventoryUI();
    }
    
    public bool HasLid()
    {
        return hasLid;
    }

    // Обновление UI инвентаря
    private void UpdateInventoryUI()
    {
         
        valveButton.SetActive(hasValve ? true : false);
        lidButton.SetActive(hasLid ? true : false);

        // Обновление гаек
        boltCountText.text = boltCount.ToString();
    }

    // Добавление гайки в инвентарь
    public void AddBolt()
    {
        boltCount++;
        boltButton.interactable = true;
        UpdateInventoryUI();
    }
    
    public void RemoveBolt()
    {
        if (boltCount != 0)
        {
            boltCount--;
        }
        
        if(boltCount <=0)
        {
            boltButton.interactable = false;
        }
        
        UpdateInventoryUI();
    }

    // Добавление крышки в инвентарь
    public void AddLidToInventory()
    {
        hasLid = true;
        UpdateInventoryUI();
    }
    public void RemoveLidFromInventory()
    {
        hasLid = false;
        UpdateInventoryUI();
    }

    public void SelectValve()
    {
       if (hasValve)
        {
            toolManager.SelectTool(ToolType.Valve);
            hasValve = false;
            UpdateInventoryUI();
            statusMessageUI.ShowHint("Выбран вентиль. Нажмите на него, чтобы установить");

            targetValve.SelectValve();
        }
        DeselectBolt();
    }
    public void DeselectValve()
    {
        if (!targetValve.IsInstalled)
        {
            targetValve.DisableValve();
            hasValve = true;
            UpdateInventoryUI();
        }
    }
    public void SelectWrench()
    {
        toolManager.SelectTool(ToolType.Wrench);
        statusMessageUI.ShowHint("Выбран гаечный ключ");
        DeselectValve();
        DeselectBolt();
    }

    public void SelectMouse()
    {
        toolManager.SelectTool(ToolType.Mouse);
        statusMessageUI.ShowHint("Выбрана мышь");
    }
    
    public void SelectBolt()
    {
       toolManager.SelectTool(ToolType.Screws);
       statusMessageUI.ShowHint("Выбрана гайка");
        
       for (int i = 0; i <  targetBolt.Length; i++)
       {
           if(!targetBolt[i].isActiveAndEnabled)
           {
                targetBolt[i].SelectBolt();
           }
       }
    }
    
    public void DeselectBolt()
    {
       for (int i = 0; i <  targetBolt.Length; i++)
       {
           if(targetBolt[i].IsSelected && !targetBolt[i].IsPlanted)
           {
                targetBolt[i].DisableBolt();
                Debug.Log("targetBolt[i].IsPlanted " + targetBolt[i].IsPlanted);
           }
       }
    }

    public void SelectLid()
    {
        if (hasLid)
        {
            targetLid.SelectLid(true);
        }
    }
    
    public void DeSelectLid()
    {
        if (hasLid)
        {
            targetLid.SelectLid(false);
        }
    }
    
}