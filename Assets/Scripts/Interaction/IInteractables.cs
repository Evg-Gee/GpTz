using System;
using UnityEngine;
// Интерфейс для всех взаимодействуемых объектов
public interface IInteractables
{
    bool CanInteract(ITool currentTool, IInventory inventory);
    void Interact(ITool currentTool, IInventory inventory);
    event Action<IInteractables> OnStateChanged;
}
