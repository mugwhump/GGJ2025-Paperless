using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public PlayerControllerScript player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveLeft()
    {
        player.moveLeft = true;
    }
    public void MoveRight()
    {
        player.moveRight = true;
    }
    public void Stop()
    {
        player.moveLeft = false;
        player.moveRight = false;
        player.doJump = false;
    }

    public void JumpBtn()
    {
        player.doJump = true;
    }
}
