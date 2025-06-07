using System.Collections;
using UnityEngine;

public class Debris : InteractableBase
{
    [SerializeField] private StatusMessage statusMessageUI;
    [SerializeField] private AnimatableBase animator;
    [SerializeField] private Lid  lidTarget;
    public override bool CanInteractWith(ToolType tool)
    {
        return tool == ToolType.Mouse && !gameObject.activeInHierarchy;
    }

    public override void Interact(ToolType tool)
    {
        if (lidTarget.IsOpen)
        {
            animator.PlayAnimation(1);
            StartCoroutine(DebrisSetActive());
            NotifyStateChanged();
            statusMessageUI.UpdateMessage("Грязь удалена!");
        }
    }
    
    private IEnumerator DebrisSetActive()
    {        
        yield return new WaitForSeconds(0.85f);
        gameObject.SetActive(false);
    }

}