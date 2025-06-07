using UnityEngine;

// Контроллер обработки кликов и взаимодействий
public class InteractionController : MonoBehaviour
{
    private InputService _input;
    private ToolManagers _toolManager;

    private void Awake()
    {
        _input = ServiceLocator.Resolve<InputService>();
        _toolManager = ServiceLocator.Resolve<ToolManagers>();
    }

    private void Update()
    {
        if (_input.WasLeftClick)
        {
            RaycastHit hit;
            if (Physics.Raycast(_input.PointerRay, out hit))
            {
                var interactable = hit.collider.GetComponent<IInteractables>();
                var tool = _toolManager.ActiveTool;
                if (interactable != null && interactable.CanInteract(tool, _toolManager.Inventory()))
                {
                    interactable.Interact(tool, _toolManager.Inventory());
                }
            }
        }
    }
}