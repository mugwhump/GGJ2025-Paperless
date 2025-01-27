using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D customCursor; 
    public Vector2 hotSpot = Vector2.zero; 

    void Start()
    {
        Cursor.SetCursor(customCursor, hotSpot, CursorMode.Auto);
    }

    //void OnDisable()
    //{
    //    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    //}
}
