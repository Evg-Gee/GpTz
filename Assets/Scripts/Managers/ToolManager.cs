using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public ToolType CurrentTool { get; private set; }
    public bool HasValveInInventory { get; private set; }

    private void Awake()
    {
        CurrentTool = ToolType.None;
        HasValveInInventory = true;
    }

    public void SelectTool(ToolType tool)
    {
        CurrentTool = tool;
    }

    public void SetValveInstalled()
    {
        HasValveInInventory = false;
    }
}