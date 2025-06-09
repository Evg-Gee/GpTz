using UnityEngine;

// Сервис запуска анимаций через Animator
public class AnimatorService : MonoBehaviour
{
    public void PlayAnimation(GameObject target, int triggerInt)
    {
        var animator = target.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetInteger("State", triggerInt);
        }
    }
}