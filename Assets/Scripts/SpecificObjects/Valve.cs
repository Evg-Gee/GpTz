using UnityEngine;

public class Valve : InteractableBase
{
    public bool IsInstalled { get; private set; } // Состояние установки
    public bool IsSelected { get; private set; } // Выбран из инвентаря
    public bool IsClosed { get; private set; }
    [SerializeField] private AnimatableBase animator;
    [SerializeField] private Material installedMaterial; // Материал после установки
    [SerializeField] private Material selectedMaterial; // Полупрозрачный материал
    [SerializeField] private ToolManager toolManager;
    [SerializeField] private StatusMessage statusMessageUI;
    [SerializeField] private Sleeve sleeve;    
    
    private BoxCollider boxCollider;
    private Material originalMaterial;

    private void Awake()
    {
        originalMaterial = GetComponent<Renderer>().material;
        boxCollider = GetComponent<BoxCollider>();
        
        DisableValve(); // Изначально вентиль не установлен
    }

    public override bool CanInteractWith(ToolType tool)
    {
        return tool == ToolType.Mouse || tool == ToolType.Valve && IsInstalled;  
    }

    public override void Interact(ToolType tool)
    {
        if (CanInteractWith(tool))
        // if (tool == ToolType.Mouse || tool == ToolType.Valve && IsInstalled)
        {
            IsClosed = !IsClosed;
            sleeve.Interact(IsClosed ? 0 : 1);
            GetComponent<Renderer>().material = installedMaterial;
            statusMessageUI.ShowHint(IsClosed ?"Вентиль открыт" : "Вентиль закрыт");
            NotifyStateChanged();
        }
    }
    
    public void OnInstall()
    {
        if (IsSelected && !IsInstalled)
        {
            InstallValve();
            toolManager.SetValveInstalled();
            statusMessageUI.ShowHint("Вентиль установлен!");
        }
    }

    // Установка вентиля
    public void InstallValve()
    {
        IsInstalled = true;
        animator.PlayAnimation(2); // Анимация установки
        GetComponent<Renderer>().material = installedMaterial;
        toolManager.SetValveInstalled(); // Обновление инвентаря
        IsClosed = true; // Изначально открыт после установки
    }

    // Выбор вентиля из инвентаря
    public void SelectValve()
    {
        IsSelected = true;
        boxCollider.enabled = true;
        animator.PlayAnimation(1);
        GetComponent<Renderer>().material = selectedMaterial; // Полупрозрачный
    }
    
    public void SelectInSceneValve()
    {
        IsSelected = true;
        boxCollider.enabled = true;
        animator.PlayAnimation(1);
    }

    public void DisableValve()
    {
        IsSelected = false;
        IsInstalled = false;
        IsClosed = true;
        boxCollider.enabled = false;
        animator.PlayAnimation(0); // Анимация установки
        GetComponent<Renderer>().material = originalMaterial;
    }
    
}