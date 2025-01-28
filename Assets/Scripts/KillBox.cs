using UnityEngine;

// Attach this to an object with a 2D collider trigger
public class KillBox : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider) {
        var p = collider.gameObject.GetComponent<PlayerControllerScript>();
        if (p != null) {
            GameManager gm = FindFirstObjectByType<GameManager>();
            gm.FuckingDie();
        }
    }
}
