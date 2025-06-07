using UnityEngine;

// Инструмент - гаечный ключ
public class WrenchTool : ITool
{
    public ToolType Type 
{ 
    get 
    { 
        return ToolType.Wrench; 
    } 
} 

    public void UseOn(IInteractables target)
{
    BoltInteractable bolt = target as BoltInteractable; // Замена is на as
    if (bolt != null) // Проверка на null
    {
        if (bolt.IsTightened && bolt.CanBeLoosened())
        {
            bolt.Loosen();
            ServiceLocator.Resolve<IInventory>().AddBolt();
        }
        else if (!bolt.IsTightened && bolt.CanBeTightened(ServiceLocator.Resolve<IInventory>()))
        {
            if (ServiceLocator.Resolve<IInventory>().UseBolt())
                bolt.Tighten();
        }
    }
}
}