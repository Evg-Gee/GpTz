 

public class BoltTool : ITool
{
    public ToolType Type
    {
        get
        { 
            return ToolType.Bolt; 
        }
    } 

    public void UseOn(IInteractables target)
    {
        throw new System.NotImplementedException();
    }
}