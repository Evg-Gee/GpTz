using UnityEngine;
 
public class Sleeve : MonoBehaviour    
{
    [SerializeField] private Animator animator;
    
    public  void PlayAnimation(int int01)
    {
        animator.SetInteger("State", int01);
    }
    
}