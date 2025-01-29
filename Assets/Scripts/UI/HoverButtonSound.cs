using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonSound: MonoBehaviour, IPointerEnterHandler 
{
    // Reference to the AudioSource on the object.
    private AudioSource hoverSound;
    
    void Awake() {
        hoverSound = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hoverSound.PlayOneShot(hoverSound.clip);
    }
}
