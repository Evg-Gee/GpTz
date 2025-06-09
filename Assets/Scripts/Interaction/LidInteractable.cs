using System;
using UnityEngine;

public class LidInteractable : MonoBehaviour, IInteractables
{
    public bool IsBoltsRemoved { get; private set; } 
    public bool IsOpen { get; private set; } 
    public bool IsPlaceholderVisible { get; private set; }
    public bool IsInInventory { get; private set; }
    public event Action<IInteractables> OnStateChanged;
    private void Awake()
    {
        IsBoltsRemoved = false;  
        IsPlaceholderVisible = false;
        IsOpen = false;  
        IsInInventory = false;
    }
   
    public bool CanInteract(ITool tool, IInventory inv)
    {
        // Если крышка в инвентаре и выбрана рука — можно установить
        if (IsInInventory && tool.Type == ToolType.Lid && IsOpen)
            return true;

        // Крышку можно снять, если все гайки откручены и выбрана "рука"
        return  IsBoltsRemoved && !IsInInventory;
    }

    public void Interact(ITool tool, IInventory inv) 
    {
        if (IsInInventory && IsOpen)
        {
            Close();
        }
        else
        {
            Toggle();
        }
    }

    public void MarkBoltsRemoved()
    {
        IsBoltsRemoved = true;
    }
    
    public void UnmarkBoltsRemoved()
    {
        IsBoltsRemoved = false;
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
    }

    private void Open()
    {
        IsOpen = true;
        IsInInventory = true;
        if (OnStateChanged != null) 
        {
            OnStateChanged(this);
        }      
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Крышка снята");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 1);
    }

    private void Close()
    {
        IsOpen = false;
        IsInInventory = false;
        IsPlaceholderVisible = false;
        if (OnStateChanged != null) 
        {
            OnStateChanged(this);
        }      
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Крышка установлена");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 0);
    }    
    public void ShowPlaceholder()
    {
        if(IsBoltsRemoved)
        {
            IsPlaceholderVisible = true;
            ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 2);
            ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Установите Крышку!"); 
        }
                    
    }
}