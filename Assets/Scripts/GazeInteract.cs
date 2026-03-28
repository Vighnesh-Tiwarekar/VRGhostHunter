using UnityEngine;
using UnityEngine.EventSystems;

public class GazeInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Renderer myRenderer;

    void Start() {
        myRenderer = GetComponent<Renderer>();
    }

    // This runs when the Reticle enters the object
    public void OnPointerEnter(PointerEventData eventData) {
        myRenderer.material.color = Color.red;
    }

    // This runs when the Reticle leaves the object
    public void OnPointerExit(PointerEventData eventData) {
        myRenderer.material.color = Color.white;
    }
}