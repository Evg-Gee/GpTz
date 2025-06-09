using System;
using UnityEngine;

public class ValveInteractable : MonoBehaviour, IInteractables
{
    public bool IsBoltsRemoved { get; private set; } 
    [SerializeField] private Sleeve sleeve;
    public bool IsInstalled { get; private set; } 
    public bool IsClosed { get; private set; }  
    public bool IsPlaceholderVisible { get; private set; }
    public event Action<IInteractables> OnStateChanged;
    private void Awake()
    {
        IsBoltsRemoved = false;  
        IsInstalled = false;  
        IsClosed = false;  
        IsPlaceholderVisible = false;
    }
    public void MarkBoltsRemoved()
    {
        IsBoltsRemoved = true;
    }
    
    public void UnmarkBoltsRemoved()
    {
        IsBoltsRemoved = false;
    }

    public bool CanInteract(ITool tool, IInventory inv)
    {
        if (IsPlaceholderVisible|| IsInstalled)  
        {
            return true;
        }
        return tool.Type == ToolType.Valve;                 
    }

    public void Interact(ITool tool, IInventory inv)
    {   
        if (IsPlaceholderVisible)
        {
            Install();
            IsPlaceholderVisible = false;
            OnStateChanged.Invoke(this);
        }
        else if (IsInstalled && IsBoltsRemoved)
        {
            Toggle();
        }
        else
        {
            Toggle();
        }            
    }  

    public void Toggle()
    {
        if (!IsInstalled)
        {
            Install();
        }
        else if (IsInstalled && !IsClosed && !IsBoltsRemoved)
        {
            Close();
        }
        else if (IsInstalled && IsClosed && !IsBoltsRemoved)
        {
            Open();
        }
        OnStateChanged.Invoke(this);
    }

    private void Install()
    {
        IsInstalled = true;
        IsPlaceholderVisible = false;
        IsClosed = false;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Вентиль установлен");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 1);
    }
    
    public void CancelPlaceholder()
    {
        if (IsInstalled)
        {
            return;
        }
        else if (IsPlaceholderVisible)
        {
            IsInstalled = false;
            IsClosed = false;
            ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Вентиль снят");
            ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 0);
        }  
    }

    private void Close()
    {
        IsClosed = true;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Вентиль закрыт");
        sleeve.PlayAnimation(1);
    }

    private void Open()
    {
        IsClosed = false;
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Вентиль открыт");
        sleeve.PlayAnimation(0);
    }
    public void ShowPlaceholder()
    {
        IsPlaceholderVisible = true;
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 4);
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Установите вентиль!");
    }
}