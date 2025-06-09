using UnityEngine;

// Инструмент - рука/крышка
public class LidTool : ITool
{
    public ToolType Type 
    { 
        get 
        { 
            return ToolType.Lid; 
        } 
    }

    public void UseOn(IInteractables target)
{
    // Замена для LidInteractable
    LidInteractable lid = target as LidInteractable;
    if (lid != null)
    {
        lid.Interact(this, ServiceLocator.Resolve<IInventory>());
    }
    else // Проверка для DirtInteractable
    {
        DirtInteractable dirt = target as DirtInteractable;
        if (dirt != null)
        {
            dirt.Interact(this, ServiceLocator.Resolve<IInventory>());
        }
    }
}
}