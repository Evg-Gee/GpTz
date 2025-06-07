using System;
using UnityEngine;
 
// Интерфейс для всех инструментов
public interface ITool
{
    ToolType Type { get; }
    void UseOn(IInteractables target);
}
