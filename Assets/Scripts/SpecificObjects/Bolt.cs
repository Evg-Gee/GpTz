using System.Collections;
using UnityEngine;

public class Bolt : InteractableBase
{
    public bool IsSelected { get; private set; }
    public bool IsLoose { get; private set; }
    public bool IsPlanted { get; private set; }
    [SerializeField] private AnimatableBase animator;
    [SerializeField] private Valve connectedValve;
    [SerializeField] private ToolMenu toolMenu;    
    [SerializeField] private Material installedMaterial;
    [SerializeField] private Material selectedMaterial;
    
    private BoxCollider boxCollider;
    private Material originalMaterial;    
    
    private void Awake()
    {
        originalMaterial = GetComponent<Renderer>().material;
        boxCollider = GetComponent<BoxCollider>();
    }
    public override bool CanInteractWith(ToolType tool)
    {
        return connectedValve.IsClosed && tool == ToolType.Wrench;
    }
    public override void Interact(ToolType tool)
    {
        if (tool == ToolType.Wrench && !connectedValve.IsClosed && !IsPlanted)
        {
            IsLoose = !IsLoose;
            animator.PlayAnimation(IsLoose ? 2 : 1);
            NotifyStateChanged();

            if (IsLoose)
            {
                toolMenu.AddBolt();
                StartCoroutine(BoltSetActive());
            }
        }
        else if(!IsPlanted && IsLoose && tool == ToolType.Screws)
        {
            IsLoose = !IsLoose;
            IsPlanted = !IsPlanted;
           
            GetComponent<Renderer>().material = originalMaterial;
            toolMenu.RemoveBolt();
            animator.PlayAnimation(IsLoose ? 2 : 1);
        }
        else if(IsPlanted && tool == ToolType.Wrench)
        {
            animator.PlayAnimation(0);
            IsPlanted = false;
            IsSelected = false;
        }
    }
    public void SelectBolt()
    {
        IsSelected = true;
        boxCollider.enabled = true;
        gameObject.SetActive(true);
        animator.PlayAnimation(3);
        GetComponent<Renderer>().material = selectedMaterial;
    }
    public void DisableBolt()
    {
        IsSelected = false;
        boxCollider.enabled = false;
        gameObject.SetActive(false);
        animator.PlayAnimation(2);
        GetComponent<Renderer>().material = originalMaterial;
    }
    IEnumerator BoltSetActive()
    {        
        yield return new WaitForSeconds (4.5f);
        gameObject.SetActive(false);
    }
}