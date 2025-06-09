using System;
using UnityEngine;

// Управление текущим выбранным инструментом
public class ToolManagers : MonoBehaviour
{
    [SerializeField] private BoltInteractable[] boltsInteractable;
    [SerializeField] private ValveInteractable valveInteractable;
    [SerializeField] private LidInteractable lidInteractable;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject valveButton;
    [SerializeField] private GameObject lidButton;
    public ITool ActiveTool { get; private set; }
    public event Action <ITool> OnToolChanged;
    public IInventory Inventory 
    {
         get 
        { 
            return ServiceLocator.Resolve<IInventory>(); 
        } 
    }
    
    private void Start()
    {
        foreach (var bolt in boltsInteractable)
        {
            bolt.OnStateChanged += OnBoltStateChanged;
            bolt.OnStateChanged += OnBoltState;
        }
        valveInteractable.OnStateChanged += ValveButtonActive;
        valveInteractable.OnStateChanged += ValveCloseChanged;
        
        lidInteractable.OnStateChanged += LidButtonActive;
    }
    
    private void ValveCloseChanged(IInteractables interactables)
    {
        if(valveInteractable.IsClosed)
        {
            foreach (var bolt in boltsInteractable)
            {
                bolt.MarkValveOpened();
            }           
        }
        else
        {
            foreach (var bolt in boltsInteractable)
            {
                bolt.UnmarkValveOpened();
                 ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Закрыть задвижку подачи ливневых вод в емкость резервуара-накопителя");
            } 
        }           
    }
    private void ValveButtonActive(IInteractables interactable)
    {
        if (valveInteractable.IsInstalled)
        {
            valveButton.SetActive(false);
        }
    }
    
    private void LidButtonActive(IInteractables interactable)
    {
        if(lidInteractable.IsOpen)
        {
            lidButton.SetActive(true);
            foreach (var bolt in boltsInteractable) 
            {
                bolt.UnmarkLidClosed();
            }
        }
        else
        {
            lidButton.SetActive(false);
            foreach (var bolt in boltsInteractable) 
            {
                bolt.MarkLidClosed();
            }
        }
    }

    // Проверяем, все ли гайки сняты после каждого отвинчивания
    private void OnBoltStateChanged(IInteractables interactable)
    {
        bool allRemoved = true;
        foreach (var bolt in boltsInteractable)
        {
            if (bolt.IsTightened)
            {
                allRemoved = false;
                break;
            }
        }
        if (allRemoved)
        {
            lidInteractable.MarkBoltsRemoved();
        }
        else
        {
            lidInteractable.UnmarkBoltsRemoved();
        }
    }
    
    private void OnBoltState(IInteractables interactables)
    {
        bool hasLooseBolt = false; // Ищем наличие хотя бы одной открученной гайки

        foreach (var bolt in boltsInteractable)
        {
            if (!bolt.IsTightened) // Если гайка откручена
            {
                hasLooseBolt = true;
                break; // Можно прервать цикл, так как уже найдена открученная гайка
            }
        }

        if (hasLooseBolt)
        {
            valveInteractable.MarkBoltsRemoved();
        }
        else // Все гайки закручены
        {
            valveInteractable.UnmarkBoltsRemoved();
        }        
    }
    
    public void SelectTool(ITool tool)
    {
        ActiveTool = tool;
        if (OnToolChanged != null)
        OnToolChanged(tool);
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage( "Выбран инструмент: " + tool.Type);
    }         
    
    public void SelectWrench()
    {
        CancelAllPlaceholders();
        SelectTool(new WrenchTool());
         ServiceLocator.Resolve<StatusMessageService>().ShowMessage( "Выбран инструмент: Гаечный ключ S24");
    } 
     
    public void ShowAllBoltPlaceholders()
    {
        var inventory = Inventory;
        if (inventory.BoltsCount <= 0 && lidInteractable.IsOpen)
        {
            ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Нет гаек в инвентаре");
            return;
        }
        foreach (var bolt in boltsInteractable)
        {
            if (!bolt.IsTightened && !bolt.IsPlaceholderVisible)
                bolt.ShowPlaceholder();
        }    
    }
    public void ShowValvePlaceholders()
    {
        SelectTool(new ValveTool());        
        valveInteractable.ShowPlaceholder();   
    }
    public void ShowLidPlaceholders()
    {
        SelectTool(new LidTool());;        
        lidInteractable.ShowPlaceholder();   
    }
    private void CancelAllPlaceholders()
    {
        foreach (var bolt in boltsInteractable)
        bolt.CancelPlaceholder();
        valveInteractable.CancelPlaceholder();
    }
}