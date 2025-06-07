using System;
using UnityEngine;

// Взаимодействие с крышкой накопителя
public class LidInteractable : MonoBehaviour, IInteractables
{
    public bool IsBoltsRemoved { get; private set; } 
    public bool IsOpen { get; private set; } 
    public event Action<IInteractables> OnStateChanged;
    private void Awake()
    {
        IsBoltsRemoved = false;  
        IsOpen = false;  
    }
    public bool CanInteract(ITool tool, IInventory inv)
    {
        // Крышку можно открыть/закрыть, если гайки сняты и выбран "рука" (ToolType.Lid)
        return tool.Type == ToolType.Lid && IsBoltsRemoved;
    }

    public void Interact(ITool tool, IInventory inv) 
    {
        Toggle();
    }

    public void MarkBoltsRemoved()
    {
        IsBoltsRemoved = true;
    }

    private void Toggle()
    {
        if (!IsOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
        if (OnStateChanged != null) // Проверка на null события
    {
        OnStateChanged(this);
    }      
    }

    private void Open()
    {
        IsOpen = true;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Крышка снята");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, "OpenLid");
    }

    private void Close()
    {
        IsOpen = false;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Крышка установлена");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, "CloseLid");
    }
}