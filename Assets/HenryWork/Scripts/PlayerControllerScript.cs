using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    // movement
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public Vector2 initColliderSize = new Vector2(1.0f, 2.63f);
    private bool isCrouching = false;

    // jump
    [SerializeField] private float jumpForce = 20.0f;
    private bool isOnGround = true;
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;

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
        playerCollider = GetComponent<BoxCollider2D>();
        Debug.Log("Initial Size: " + playerCollider.size);

        actionButtonA = "MoveLeft";
        actionButtonD = "SprintRight";
        actionButtonK = "Jump";
        actionButtonL = "Crouch";
    }

    void Update()
    {
        // set button color
        buttonA.targetGraphic.color = Input.GetKey(KeyCode.A) ? buttonA.colors.pressedColor : buttonA.colors.normalColor;
        buttonD.targetGraphic.color = Input.GetKey(KeyCode.D) ? buttonD.colors.pressedColor : buttonD.colors.normalColor;
        buttonK.targetGraphic.color = Input.GetKey(KeyCode.K) ? buttonK.colors.pressedColor : buttonK.colors.normalColor;
        buttonL.targetGraphic.color = Input.GetKey(KeyCode.L) ? buttonL.colors.pressedColor : buttonL.colors.normalColor;

        if (!isOnGround)
        {
            playerCollider.size = new Vector2(1.0f, 3.0f);
            Debug.Log(playerCollider.size);
        }
        else if (isOnGround && !isCrouching)
        {
            playerCollider.size = initColliderSize;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            ActionHandler(actionButtonA);
        }

        if (Input.GetKey(KeyCode.D))
        {
            ActionHandler(actionButtonD);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ActionHandler(actionButtonK);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ActionHandler(actionButtonL);
            Debug.Log(playerCollider.size);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.L))
        {
            ResetPlayer();
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
                HandleCrouch(new Vector2(1.0f, 1.0f));
                break;
            default:
                ResetPlayer();
                break;
        }

    }

    // -------------------- action functions -------------------- // 

    private void ResetPlayer()
    {
        playerCollider.size = initColliderSize;
        playerCollider.offset = new Vector2(0f, 0f);
        jumpForce = 20f;
        isCrouching = false;
    }
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
        jumpForce = 30f;
        transform.Translate(Vector3.left * sprintSpeed * Time.deltaTime);

    }
    private void HandleSprintRight()
    {
        jumpForce = 30f;
        transform.Translate(Vector3.right * sprintSpeed * Time.deltaTime);
    }
    private void HandleJump()
    {
        if (isOnGround)
        {
            Jump();
        }
    }
    private void HandleCrouch(Vector2 newSize)
    {
        isCrouching = true;
        playerCollider.offset = new Vector2(0f, -0.7f);
        playerCollider.size = newSize;

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
