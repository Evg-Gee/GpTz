using System.Collections;
using System.Linq;
using UnityEngine;

public class Lid : InteractableBase
{
    public bool IsOpen { get; private set; } // Состояние установки
    public bool IsSelected { get; private set; } // Выбран из инвентаря
    [SerializeField] private AnimatableBase animator;
    [SerializeField] private Bolt[] bolts; // Массив гаек
    [SerializeField] private ToolMenu toolMenu; // Ссылка на инвентарь
    [SerializeField] private Material installedMaterial; // Материал после установки
    [SerializeField] private Material selectedMaterial; // Полупрозрачный материал
    [SerializeField] private ToolManager toolManager;
    [SerializeField] private StatusMessage statusMessageUI;
    private BoxCollider boxCollider;

    private Material originalMaterial;
    
     private void Awake()
    {
        originalMaterial = GetComponent<Renderer>().material;
        boxCollider = GetComponent<BoxCollider>();
    }
    
    public override bool CanInteractWith(ToolType tool)
    {
        return bolts.All(b => b.IsLoose) 
            && !toolMenu.HasLid();
    }

    public override void Interact(ToolType tool)
    {
        if (CanInteractWith(tool))
        {            
            animator.PlayAnimation(1);
            NotifyStateChanged();

            toolMenu.AddLidToInventory();
            statusMessageUI.ShowHint("Крышка снята");
            Debug.Log("Крышка снята");

            StartCoroutine(LidSetActive());
            IsOpen = true;
        }
        else if (IsSelected && toolMenu.HasLid() && IsOpen)
        {
            Debug.Log("Ставим крышку");
            PlaceLid();
        }
    }
    
    private IEnumerator LidSetActive()
    {        
        yield return new WaitForSeconds(2.2f);
        gameObject.SetActive(false);
    }

   public void PlaceLid()
    {
        if (toolMenu.HasLid())
        {
            IsOpen = false;  
            IsSelected = false;          
            animator.PlayAnimation(0);
             GetComponent<Renderer>().material = installedMaterial;
            NotifyStateChanged();

            toolMenu.RemoveLidFromInventory();
        }
    }
    public void SelectLid(bool setSelect)
    {
        IsSelected = setSelect;
        boxCollider.enabled = setSelect;
        gameObject.SetActive(setSelect);
        animator.PlayAnimation(0);
        GetComponent<Renderer>().material = selectedMaterial;
    }
    
}