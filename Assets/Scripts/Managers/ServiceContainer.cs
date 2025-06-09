using UnityEngine;

public class ServiceContainer : MonoBehaviour 
{

    void Awake()
    {
        ServiceLocator.Register(InputServiceLocator());
        ServiceLocator.Register(ToolManagerLocator());
        ServiceLocator.Register(InventoryLocator());
        ServiceLocator.Register(StatusMessageLocator());
        ServiceLocator.Register(AnimatorLocator());

    }
    private InputService InputServiceLocator()
    {
        return FindObjectOfType<InputService>();
    }

    private ToolManagers ToolManagerLocator()
    {
        return FindObjectOfType<ToolManagers>();
    }

    private IInventory InventoryLocator()
    {
        return FindObjectOfType<Inventory>();
    }

    private StatusMessageService StatusMessageLocator()
    {
        return FindObjectOfType<StatusMessageService>();
    }

    private AnimatorService AnimatorLocator()
    {
        return FindObjectOfType<AnimatorService>();
    }
}