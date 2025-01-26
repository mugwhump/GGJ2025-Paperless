using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    // movement
    public float moveSpeed = 5f;
    public float pushForce = 100000;
    private bool isCrouching = false;

    private bool facingRight = true;
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRb;

    // jump
    [SerializeField] private float jumpForce = 20.0f;
    private Vector2 initColliderSize; //modify via the editor, store the initial dimensions
    private Vector2 initColliderOffset; //modify via the editor, store the initial dimensions
    // ground check for jump
    private bool isOnGround = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

    // set button action
    public string actionButtonA;
    public string actionButtonD;
    public string actionButtonK;
    public string actionButtonL;

    // Animation
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _lastPosition;

    // UI
    // [SerializeField] private Button buttonA;
    // [SerializeField] private Button buttonD;
    // [SerializeField] private Button buttonK;
    // [SerializeField] private Button buttonL;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        Debug.Log("Initial Size: " + playerCollider.size); 
        initColliderSize = playerCollider.size;       
        initColliderOffset = playerCollider.offset;
        
        // GameObject bc = GameObject.FindWithTag("ButtonContainerTag");
        // if(bc != null) {
        //     buttonA = bc.gameObject.transform.GetChild(0).GetComponent<Button>();
        //     buttonD = bc.gameObject.transform.GetChild(1).GetComponent<Button>();
        //     buttonK = bc.gameObject.transform.GetChild(2).GetComponent<Button>();
        //     buttonL = bc.gameObject.transform.GetChild(3).GetComponent<Button>();
        // }
        // else {
        //     Debug.Log("Couldn't find Object with ButtonContainer tag!");
        // }
        //Store the initial collider size

        actionButtonA = "MoveLeft";
        actionButtonD = "MoveRight";
        actionButtonK = "Jump";
        actionButtonL = "Crouch";
    }

    void Update()
    {
        isOnGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.5f, 1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        //if (Input.GetKey(KeyCode.X) && isOnGround)
        //{
        //    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
        //}
        // // set button color
        // buttonA.targetGraphic.color = Input.GetKey(KeyCode.A) ? buttonA.colors.pressedColor : buttonA.colors.normalColor;
        // buttonD.targetGraphic.color = Input.GetKey(KeyCode.D) ? buttonD.colors.pressedColor : buttonD.colors.normalColor;
        // buttonK.targetGraphic.color = Input.GetKey(KeyCode.K) ? buttonK.colors.pressedColor : buttonK.colors.normalColor;
        // buttonL.targetGraphic.color = Input.GetKey(KeyCode.L) ? buttonL.colors.pressedColor : buttonL.colors.normalColor;


        if (isOnGround && !isCrouching)
        {
            playerCollider.size = initColliderSize;
            playerCollider.offset = initColliderOffset;
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

        // Update the rotation
        if (facingRight)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
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
            case "Push":
                HandlePush();
                break;
            default:
                ResetPlayer();
                break;
        }

        // Compare only x and y components for 2D movement
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        if(_lastPosition != currentPosition) 
        {
            if(isOnGround) 
            {
                _animator.SetBool(IsMoving, true);
            }
        }
        else
        {
            _animator.SetBool(IsMoving, false);
        }
        _lastPosition = currentPosition;
    }

    // -------------------- action functions -------------------- // 

    private void ResetPlayer()
    {
        moveSpeed = 0;
        playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
        playerCollider.size = initColliderSize;
        //playerCollider.offset = new Vector2(0f, 0f); //better to handle the offset via the editor
        jumpForce = 20f;
        isCrouching = false;
    }
    private void HandleInputLeft()
    {
        moveSpeed = -5f;
        playerRb.linearVelocity = new Vector2(moveSpeed, playerRb.linearVelocity.y);
        facingRight = false;
    }

    private void HandleInputRight()
    {
        moveSpeed = 5f;
        playerRb.linearVelocity = new Vector2(moveSpeed, playerRb.linearVelocity.y);
        facingRight = true;
    }

    private void HandleSprintLeft()
    {
        moveSpeed = -10f;
        jumpForce = 30f;
        playerRb.linearVelocity = new Vector2(moveSpeed, playerRb.linearVelocity.y);
        facingRight = false;
    }

    private void HandleSprintRight()
    {
        moveSpeed = 10f;
        jumpForce = 30f;
        playerRb.linearVelocity = new Vector2(moveSpeed, playerRb.linearVelocity.y);
        facingRight = true;
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
        //playerCollider.offset = new Vector2(0f, -0.7f);
        playerCollider.size = newSize;

    }

    private void HandlePush() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (facingRight ? Vector3.right : Vector3.left), 
        (facingRight ? Vector2.right : Vector2.left) * 3, moveSpeed * 3); 
        if(hit && hit.transform.gameObject.CompareTag("Pushable")) {
            //Debug.Log(hit);
            if(hit.transform.GetComponent<PlayerControllerScript>() != null) {
                //Debug.Log("aw fug");
            }
            else {
                hit.transform.Translate((facingRight ? Vector3.right : Vector3.left) * moveSpeed * 2 * Time.deltaTime);
                Debug.Log("Push!");
                Rigidbody2D rb = hit.transform.GetComponent<Rigidbody2D>();
                if(rb != null){
                    rb.AddForceX(pushForce * (facingRight ? 1 : -1));
                }            
            }
        }
    }

    public void Jump()
    {
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        Debug.Log("ground");
    //        isOnGround = true;
    //    }
    //}
}
