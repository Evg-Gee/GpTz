using UnityEngine;

public class HoverHighlight : MonoBehaviour 
{
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float transitionDuration = 0.2f;
    
    private MaterialPropertyBlock _propBlock;

    private Renderer[] renderers;
    private Color[] originalColors;
    private bool isHighlighted;

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;
    }

    private void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (isHighlighted) SetHighlight(false);
            return;
        }
        #endif
        
        Ray ray = ServiceLocator.Resolve<InputService>().PointerRay;
        RaycastHit hit;
        bool hitThis = Physics.Raycast(ray, out hit) && hit.collider == GetComponent<Collider>();

        if (hitThis && !isHighlighted)
            SetHighlight(true);
        else if (!hitThis && isHighlighted)
            SetHighlight(false);
    }

    private void SetHighlight(bool highlight)
    {
        isHighlighted = highlight;
        StopAllCoroutines();
        for (int i = 0; i < renderers.Length; i++)
        {
            ColorTransition(renderers[i], highlight ? highlightColor : originalColors[i]);
        }
    }

    private void ColorTransition(Renderer ren, Color target)
{
    ren.GetPropertyBlock(_propBlock);
    _propBlock.SetColor("_Color", target);
    ren.SetPropertyBlock(_propBlock);
}
}