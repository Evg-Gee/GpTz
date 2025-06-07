using UnityEngine;

public class AnimatableBase : MonoBehaviour, IAnimatable
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected string animationName;

    public virtual void PlayAnimation(int animationInt)
    {
        animator.SetInteger("State", animationInt);
    }
}