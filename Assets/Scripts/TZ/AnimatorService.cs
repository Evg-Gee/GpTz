using UnityEngine;

// Сервис запуска анимаций через Animator
public class AnimatorService : MonoBehaviour
{
    public void PlayAnimation(GameObject target, string trigger)
    {
        var animator = target.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(trigger);
        }
    }
}