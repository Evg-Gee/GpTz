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
    BoltInteractable bolt = target as BoltInteractable; 
    if (bolt != null)  
    {
        if (bolt.IsTightened)
        {
           if (bolt.CanBeLoosened())
                {
                    bolt.Loosen();
                    ServiceLocator.Resolve<IInventory>().AddBolt();
                }
        }
        else if (bolt.IsPlacing)
            {
                if (ServiceLocator.Resolve<IInventory>().UseBolt() == true)
                {
                    bolt.Tighten();
                }
            }
    }
}
}