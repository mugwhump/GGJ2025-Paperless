using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    // movement
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public bool moveLeft = false;
    public bool moveRight = false;
    public bool doJump = false;

    // jump
    [SerializeField] private float jumpForce = 5.0f;
    private bool isOnGround = true;
    private Rigidbody2D playerRb;

    // set button action
    public string actionButtonA;
    public string actionButtonD;
    public string actionButtonK;
    public string actionButtonL;


    // UI
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonD;
    [SerializeField] private Button buttonK;
    [SerializeField] private Button buttonL;


    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        actionButtonA = "MoveLeft";
        actionButtonD = "SprintRight";
        actionButtonK = "Jump";
        actionButtonL = "Jump";
    }

    void Update()
    {
        // set button color
        buttonA.targetGraphic.color = Input.GetKey(KeyCode.A) ? buttonA.colors.pressedColor : buttonA.colors.normalColor;
        buttonD.targetGraphic.color = Input.GetKey(KeyCode.D) ? buttonD.colors.pressedColor : buttonD.colors.normalColor;
        buttonK.targetGraphic.color = Input.GetKey(KeyCode.K) ? buttonK.colors.pressedColor : buttonK.colors.normalColor;
        buttonL.targetGraphic.color = Input.GetKey(KeyCode.L) ? buttonL.colors.pressedColor : buttonL.colors.normalColor;

        
        if (Input.GetKey(KeyCode.A))
        {
            ActionHandler(actionButtonA);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ActionHandler(actionButtonD);
        }
        if (Input.GetKey(KeyCode.K))
        {
            ActionHandler(actionButtonK);
        }
        if (Input.GetKey(KeyCode.L))
        {
            ActionHandler(actionButtonL);
        }
    }


    private void ActionHandler(string action)
    {
        switch (action)
        {
            case "MoveLeft":
                HandleInputLeft();
                break;
            case "MoveRight":
                HandleInputRight(); 
                break;
            case "SprintLeft":
                HandleSprintLeft();
                break;
            case "SprintRight":
                HandleSprintRight();
                break;
            case "Jump":
                HandleJump();
                break;
            case "Crouch":
                HandleCrouch();
                break;
            default:
                break;
        }

    }

    // -------------------- action functions -------------------- // 
    private void HandleInputLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void HandleInputRight()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
    private void HandleSprintLeft()
    {
        transform.Translate(Vector3.left * sprintSpeed * Time.deltaTime);
        jumpForce = 7f;
    }
    private void HandleSprintRight()
    {
        transform.Translate(Vector3.right * sprintSpeed * Time.deltaTime);
        jumpForce = 7f;
    }
    private void HandleJump()
    {
        if (isOnGround)
        {
            Jump();
        }
    }
    private void HandleCrouch()
    {

    }

    public void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        isOnGround = false;
        playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, jumpForce, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

    }
}
