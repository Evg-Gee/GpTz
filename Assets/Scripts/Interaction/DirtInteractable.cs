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
        // Грязь можно убрать, если крышка  
        var lid = FindObjectOfType<LidInteractable>();
        return  lid != null && lid.IsOpen && !IsCleaned;
    }

    public void Interact(ITool tool, IInventory inv)
    {
        Clean();
        if (OnStateChanged != null)  
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
            animator.PlayAnimation(gameObject, 1);
        }

        // Уведомление через StatusMessage
        var statusMessageUI = GameObject.FindObjectOfType<StatusMessageService>();
        if (statusMessageUI != null)
        {
            statusMessageUI.ShowMessage("Грязь убрана");
        }
        
        if (OnStateChanged != null)  
        {
            OnStateChanged(this);
        }

        Destroy(gameObject.gameObject);
    }
}