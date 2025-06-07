using System;
using UnityEngine;

// Взаимодействие с грязью внутри накопителя
public class DirtInteractable : MonoBehaviour, IInteractables
{
    public bool IsCleaned { get; private set; }
    public event Action<IInteractables> OnStateChanged;
    
    private void Awake()
    {
        IsCleaned = false;  
    }

    public bool CanInteract(ITool tool, IInventory inv)
    {
        // Грязь можно убрать, если крышка открыта и выбран "рука" (ToolType.Lid)
        var lid = FindObjectOfType<LidInteractable>();
        return tool.Type == ToolType.Lid && lid != null && lid.IsOpen && !IsCleaned;
    }

    public void Interact(ITool tool, IInventory inv)
    {
        Clean();
        if (OnStateChanged != null) // Проверка на null события
        {
            OnStateChanged(this);
        }
    }

    private void Clean()
    {
        IsCleaned = true;
    
        // Воспроизвести анимацию через компонент
        var animator = GetComponent<AnimatorService>();
        if (animator != null)
        {
            animator.PlayAnimation(gameObject, "CleanDirt");
        }

        // Уведомление через StatusMessage
        var statusMessageUI = GameObject.FindObjectOfType<StatusMessageService>();
        if (statusMessageUI != null)
        {
            statusMessageUI.ShowMessage("Грязь убрана");
        }

        Destroy(gameObject);
        // IsCleaned = true;
        // ServiceLocator.Resolve<StatusMessageService>()?.ShowMessage("Грязь убрана");
        // ServiceLocator.Resolve<AnimatorService>()?.PlayAnimation(gameObject, "CleanDirt");
        // Destroy(gameObject);
    }
}