using UnityEngine;

public class HoverHighlight : MonoBehaviour 
{
	[SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnMouseEnter()
    {
        meshRenderer.material = highlightMaterial;
    }

    void OnMouseExit()
    {
        meshRenderer.material = defaultMaterial;
    }
}
