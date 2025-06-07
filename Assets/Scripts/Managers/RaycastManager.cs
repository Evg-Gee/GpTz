using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private ToolManager toolManager;
    [SerializeField] private StatusMessage statusMessageUI;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();

                Valve valve = interactable as Valve;
                if (valve != null && valve.IsSelected && !valve.IsInstalled)
                {
                    valve.OnInstall();
                }
                else if (valve != null && valve.IsInstalled)
                {
                    toolManager.SelectTool(ToolType.Mouse);
                    valve.Interact(toolManager.CurrentTool);
                }
                else if (interactable != null)
                {
                    interactable.Interact(toolManager.CurrentTool);
                    Debug.Log("interactable.Interact(toolManager.CurrentTool);" + toolManager.CurrentTool);
                }
            }
        }
    }
}