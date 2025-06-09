using System;
using UnityEngine;

public class BoltInteractable : MonoBehaviour, IInteractables
{
    [SerializeField] private Renderer boltRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material placeholderMaterial;
    
    public bool IsTightened { get; private set; }
    public bool IsPlacing { get; private set; }
    public bool IsPlaceholderVisible { get; private set; }
    public bool IsValveClosed { get; private set; }
    public bool IsLidClosed { get; private set; }
    public event Action <IInteractables> OnStateChanged;
    private void Awake()
    {
        IsTightened = true; 
        IsLidClosed = true; 
        IsValveClosed = false; 
        IsPlacing = false;
        IsPlaceholderVisible = false;
    }
    
    public void MarkValveOpened()
    {
        IsValveClosed = true;
    }
    
    public void UnmarkValveOpened ()
    {
        IsValveClosed = false;
    }
    
    public void MarkLidClosed ()
    {
        IsLidClosed = true;
    }
    public void UnmarkLidClosed ()
    {
        IsLidClosed = false;
    }
    public bool CanInteract(ITool tool, IInventory inv)
    {
        if (IsPlaceholderVisible)
            return true;

        switch (tool.Type)
        {
            case ToolType.Wrench:
                return (IsValveClosed && IsPlacing) || (IsValveClosed && IsTightened && CanBeLoosened());
            case ToolType.Lid:
                return IsValveClosed && !IsTightened && !IsPlaceholderVisible && inv.BoltsCount > 0;
            default:
                return false;
        }
    }

    public bool CanBeLoosened() 
    { 
        return IsTightened; 
    }    
    
    public void Interact(ITool tool, IInventory inv) 
    {   
        if (IsPlaceholderVisible)
        {
            Placing();
            return;
        }

        switch (tool.Type)
        {
            case ToolType.Wrench:
                if (IsTightened && CanBeLoosened())
                {
                    Loosen();
                    inv.AddBolt();
                }
                else if (!IsTightened && IsPlacing && inv.UseBolt())
                {
                    Tighten();
                }
                break;

            case ToolType.Lid:
                if (!IsTightened && !IsPlaceholderVisible && inv.BoltsCount > 0 )
                {
                    ShowPlaceholder();
                }
                break;
        }
    }

    public void Loosen()
    {
        IsTightened = false;
        if (OnStateChanged != null) 
        {
            OnStateChanged(this);
        }
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 2);
    }

    public void Tighten()
    {
        IsTightened = true;
        IsPlacing = false;
        if (OnStateChanged != null) 
        {
            OnStateChanged(this);
        }
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 0);
        boltRenderer.material = defaultMaterial;
    }
    public void Placing()
    {
        IsPlacing = true;
        IsPlaceholderVisible = false;
        if (OnStateChanged != null) 
        {
            OnStateChanged(this);
        }
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 1);
        boltRenderer.material = defaultMaterial;
    }
    public void ShowPlaceholder()
    {
        if(IsLidClosed)
        {
            IsPlaceholderVisible = true;
            if (OnStateChanged != null) 
        {
            OnStateChanged(this);
        }
            ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 3);
            boltRenderer.material = placeholderMaterial;         
            ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Гайка готова к наживлению");
        }        
    }
    public void CancelPlaceholder()
    {
        if (!IsPlaceholderVisible)
        {
            return;
        } 
        IsPlaceholderVisible = false;
        OnStateChanged.Invoke(this);
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Наживление отменено");
        ServiceLocator.Resolve<AnimatorService>().PlayAnimation(gameObject, 2);
        boltRenderer.material = defaultMaterial;
    }
}