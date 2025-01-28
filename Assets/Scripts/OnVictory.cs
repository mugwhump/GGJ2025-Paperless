using System;
using UnityEngine;

public class OnVictory : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup victoryCanvasGroup;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            victoryCanvasGroup.alpha = 1;
            victoryCanvasGroup.blocksRaycasts = true;
            victoryCanvasGroup.interactable = true;
            AudioSource winMusic = GetComponent<AudioSource>();
            winMusic.PlayOneShot(winMusic.clip);
        }
    }
}
