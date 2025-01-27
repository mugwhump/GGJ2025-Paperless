using GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    private static readonly int IS_MOVING_ANIMATOR = Animator.StringToHash("isWalking");
    private static readonly int IS_GROUNDED_ANIMATOR = Animator.StringToHash("isGrounded");
    private static readonly int IS_CROUCHING_STATIC_ANIMATOR = Animator.StringToHash("isCrouchingStatic");
    private static readonly int IS_CROUCHING_WALKING_ANIMATOR = Animator.StringToHash("isCrouchingWalking");
    private static readonly int jumpTrig = Animator.StringToHash("JumpTrigger");

    // movement
    public float moveSpeed = 0f;
    public float pushForce = 100000;
    private bool isCrouching = false;
    private float horizontalInput;

    private bool facingRight = true;
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRb;

    // jump
    [SerializeField] private float jumpForce = 25.0f;
    private Vector2 initColliderSize; //modify via the editor, store the initial dimensions
    private Vector2 initColliderOffset; //modify via the editor, store the initial dimensions
    // ground check for jump
    private bool isOnGround = true;

    // set button action

    public string actionButtonK;
    public string actionButtonL;

    // Animation
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _lastPosition;

    // UI
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Image[] buttonImgs;

    private ContactFilter2D contactFilter;

    public bool IsGrounded() => playerRb.IsTouching(contactFilter);


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

        contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        
 
        actionButtonK = "Jump";
        actionButtonL = "Crouch";
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        playerRb.linearVelocity = new Vector2(moveSpeed * horizontalInput, playerRb.linearVelocity.y);
        facingRight = horizontalInput > 0 ? true : false;

        // ================ Animator ==================================================================================
        if (horizontalInput != 0f)
        {
            if (IsGrounded())
            {
                _animator.SetBool(IS_MOVING_ANIMATOR, true);
                //playerCollider.offset = new Vector2(playerCollider.offset.x, -0.5f);
            }
        }
        else
        {
            _animator.SetBool(IS_MOVING_ANIMATOR, false);
            //playerCollider.offset = initColliderOffset;
        }
        
        _animator.SetBool(IS_GROUNDED_ANIMATOR, isOnGround);

        //isOnGround = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.5f, 1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        isOnGround = IsGrounded();

        if (isOnGround && !isCrouching)
        {
            playerCollider.size = initColliderSize;
            playerCollider.offset = initColliderOffset;
        }

   

        if (Input.GetKeyDown(KeyCode.K))
        {
            ActionHandler(Inventory.Instance?.GetSlotAt(0)?.keyword?.keywordString);
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            ActionHandler(Inventory.Instance?.GetSlotAt(1)?.keyword?.keywordString);
        }


        if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.L))
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

        if(isCrouching) {
            if(horizontalInput != 0f) {
                Debug.Log("Crouch walking");
                 _animator.SetBool(IS_CROUCHING_WALKING_ANIMATOR, true);
                _animator.SetBool(IS_CROUCHING_STATIC_ANIMATOR, false); 
            }
            else {
                Debug.Log("Crouch");
                _animator.SetBool(IS_CROUCHING_STATIC_ANIMATOR, true);  
                _animator.SetBool(IS_CROUCHING_WALKING_ANIMATOR, false); 
            }
        }
    }

    private void ActionHandler(string action)
    {
        switch (action)
        {
            case "Sprint":
                HandleSprint();
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
        //Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        //if(_lastPosition != currentPosition) 
        //_lastPosition = currentPosition;
    }

    // -------------------- action functions -------------------- // 

    private void ResetPlayer()
    {
        moveSpeed = 5f;
        playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
        playerCollider.size = initColliderSize;
        jumpForce = 25f;
        isCrouching = false;

    }


    private void HandleSprint()
    {
        moveSpeed = 10f;
        jumpForce = 30f;
       
    }

 
    private void HandleJump()
    {
        //if (isOnGround)
        if(IsGrounded()) 
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up + (facingRight ? Vector3.right : Vector3.left), 
        (facingRight ? Vector2.right : Vector2.left) * 3, 9); 
        Debug.Log("Checking push!");
        if(hit && hit.transform.gameObject.CompareTag("Pushable")) {
            //Debug.Log(hit);
            if(hit.transform.GetComponent<PlayerControllerScript>() != null) {
                //Debug.Log("aw fug");
            }
            else {
                hit.transform.Translate((facingRight ? Vector3.right : Vector3.left) * 6 * Time.deltaTime);
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
        _animator.SetTrigger("JumpTrigger");
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
