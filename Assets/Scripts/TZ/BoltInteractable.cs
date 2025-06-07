using System;
using UnityEngine;

// Взаимодействие с гайкой
public class BoltInteractable : MonoBehaviour, IInteractables
{
    public bool IsTightened { get; private set; }
    public event Action <IInteractables> OnStateChanged;
    private void Awake()
    {
        IsTightened = true; 
    }

    public bool CanInteract(ITool tool, IInventory inv)
    {
        return tool.Type == ToolType.Wrench &&
               ((IsTightened && CanBeLoosened()) || (!IsTightened && CanBeTightened(inv)));
    }

    public bool CanBeLoosened() 
    { 
        return IsTightened; 
    }
    public bool CanBeTightened(IInventory inv) 
    { 
        return !IsTightened && inv.BoltsCount > 0; 
    }

    public void Interact(ITool tool, IInventory inv) 
    { 
        tool.UseOn(this); 
    }

    public void Loosen()
    {
        IsTightened = false;
        if (OnStateChanged != null) // Проверка на null события
        {
            OnStateChanged(this);
        }
        // Запустить анимацию отвинчивания
    }

    public void Tighten()
    {
        IsTightened = true;
        if (OnStateChanged != null) // Проверка на null события
        {
            OnStateChanged(this);
        }
        // Запустить анимацию завинчивания
    }
}