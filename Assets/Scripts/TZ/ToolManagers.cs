using UnityEngine;

// Управление текущим выбранным инструментом
public class ToolManagers : MonoBehaviour
{
    public ITool ActiveTool { get; private set; }
    public IInventory Inventory() 
    {
        return ServiceLocator.Resolve<IInventory>();

    }
    public void SelectTool(ITool tool)
    {
        ActiveTool = tool;
        Debug.Log("Выбран инструмент: " + tool.Type);
        ServiceLocator.Resolve<StatusMessageService>().ShowMessage( "Инструмент: " + tool.Type);
    }
}