using UnityEngine;

// Инструмент - вентиль
public class ValveTool : ITool
{
    public ToolType Type 
{ 
    get 
    { 
        return ToolType.Valve; 
    } 
}

    public void UseOn(IInteractables target)
    {
        ValveInteractable valve = target as ValveInteractable;
        if (valve != null)
        {
            valve.Toggle();
        }
    }
}