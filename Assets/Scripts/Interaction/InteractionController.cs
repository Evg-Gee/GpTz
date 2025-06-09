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
     if (_input.WasLeftClick())
        {
            RaycastHit hit;
            bool interacted = false;

            if (Physics.Raycast(_input.PointerRay, out hit))
            {
                var interactable = hit.collider.GetComponent<IInteractables>();
                var tool = _toolManager.ActiveTool;

                if (interactable != null && interactable.CanInteract(tool, _toolManager.Inventory))
                {
                    interactable.Interact(tool, _toolManager.Inventory);
                    interacted = true;
                }
            }

            // Если placeholder-гайка активна, но не было взаимодействия — отменяем placeholder для всех гаек
            if (!interacted)
            {
                var bolts = FindObjectsOfType<BoltInteractable>();
                foreach (var bolt in bolts)
                {
                    bolt.CancelPlaceholder();
                }
            }
        }
        // if (_input.WasLeftClick())
        // {
        //     RaycastHit hit;
        //     if (Physics.Raycast(_input.PointerRay, out hit))
        //     {
        //         var interactable = hit.collider.GetComponent<IInteractables>();
        //         var tool = _toolManager.ActiveTool;
        //         if (interactable != null && interactable.CanInteract(tool, _toolManager.Inventory))
        //         {
        //             interactable.Interact(tool, _toolManager.Inventory);
        //         }
        //     }
        // }
    }
}