using UnityEngine;
using UnityEngine.EventSystems;

public class GazeInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Renderer myRenderer;

    void Start() {
        myRenderer = GetComponent<Renderer>();
        if (myRenderer == null) {
            Debug.LogWarning("GazeInteract: No Renderer component found on " + gameObject.name);
            enabled = false;
            return;
        }
        // Create instance to avoid modifying shared material
        myRenderer.material = new Material(myRenderer.material);
    }

    // This runs when the Reticle enters the object
    public void OnPointerEnter(PointerEventData eventData) {
        if (myRenderer != null)
            myRenderer.material.color = Color.red;
    }

    // This runs when the Reticle leaves the object
    public void OnPointerExit(PointerEventData eventData) {
        if (myRenderer != null)
            myRenderer.material.color = Color.white;
    }
}