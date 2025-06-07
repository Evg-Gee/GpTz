using System;

public interface IInteractable
{
    bool CanInteractWith(ToolType tool);
    void Interact(ToolType tool);
    event Action<IInteractable> OnStateChanged;
}