using System;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    public virtual bool CanInteractWith(ToolType tool)
    {
        return false;
    }
    public virtual void Interact(ToolType tool) { }

    public event Action<IInteractable> OnStateChanged;

    protected void NotifyStateChanged()
    {
        if (OnStateChanged != null) 
        {
            OnStateChanged(this);
        }
    }
}