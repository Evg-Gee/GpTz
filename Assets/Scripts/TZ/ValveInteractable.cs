using System;
using UnityEngine;

// Взаимодействие с вентилем
public class ValveInteractable : MonoBehaviour, IInteractables
{
    public bool IsInstalled { get; private set; } 
    public bool IsClosed { get; private set; }  
    public event Action<IInteractables> OnStateChanged;
    private void Awake()
    {
        IsInstalled = false;  
        IsClosed = false;  
    }

    public bool CanInteract(ITool tool, IInventory inv)
    {
        // Вентиль можно установить/снять без дополнительных условий
        return tool.Type == ToolType.Valve;
    }

    public void Interact(ITool tool, IInventory inv)
    {
        tool.UseOn(this);
    }  

    // Установить или снять вентиль
    public void Toggle()
    {
        if (!IsInstalled)
        {
            Install();
        }
        else if (IsInstalled && !IsClosed)
        {
            Close();
        }
        else if (IsInstalled && IsClosed)
        {
            Open();
        }
        OnStateChanged.Invoke(this);
    }

    private void Install()
    {
        IsInstalled = true;
        IsClosed = false;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Вентиль установлен");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, "InstallValve");
    }

    private void Close()
    {
        IsClosed = true;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Вентиль закрыт");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, "CloseValve");
    }

    private void Open()
    {
        IsClosed = false;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Вентиль открыт");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, "OpenValve");
    }
}