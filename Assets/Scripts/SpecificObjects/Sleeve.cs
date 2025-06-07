using UnityEngine;

public class Sleeve : InteractableBase
{
    [SerializeField] private AnimatableBase animator;
    public  void Interact(int int01)
    {
        animator.PlayAnimation(int01);
    }
    
}