using GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour
{
    private static readonly int IS_WALKING_ANIMATOR = Animator.StringToHash("isWalking");
    private static readonly int IS_GROUNDED_ANIMATOR = Animator.StringToHash("isGrounded");
    private static readonly int IS_CROUCHING_STATIC_ANIMATOR = Animator.StringToHash("isCrouchingStatic");
    private static readonly int IS_CROUCHING_WALKING_ANIMATOR = Animator.StringToHash("isCrouchingWalking");
    private static readonly int IS_SPRINTING_ANIMATOR = Animator.StringToHash("isSprinting");
    private static readonly int jumpTrig = Animator.StringToHash("JumpTrigger");

    // movement
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float pushForce = 100000;
    private bool isCrouching = false;
    private bool isSprinting = false;
    public float horizontalInput; //TODO: debugging, make private again

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

    // Audio
    private AudioSource audioSource;
    public AudioClip jumpAudio;
    public AudioClip landAudio;

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
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Initial Size: " + playerCollider.size); 
        initColliderSize = playerCollider.size;       
        initColliderOffset = playerCollider.offset;

        contactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        
 
        actionButtonK = "";
        actionButtonL = "";
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        playerRb.linearVelocity = new Vector2(currentSpeed * horizontalInput, playerRb.linearVelocity.y);
        
        if(horizontalInput > 0){
            facingRight = true;
        }
        else if(horizontalInput < 0){
            facingRight = false;
        }

        
        if (Input.GetKeyDown(KeyCode.K))
        {
            ActionHandler(Inventory.Instance?.GetSlotAt(0)?.keyword?.keywordString);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ActionHandler(Inventory.Instance?.GetSlotAt(1)?.keyword?.keywordString);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            KeyUpHandler(Inventory.Instance?.GetSlotAt(0)?.keyword?.keywordString);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            KeyUpHandler(Inventory.Instance?.GetSlotAt(1)?.keyword?.keywordString);
        }

        isOnGround = IsGrounded();

        if(!isOnGround) {
            unCrouch();
        }

        // ================ Animator ==================================================================================
        if (isOnGround && !isCrouching)
        {
            if(isSprinting) {
                _animator.SetBool(IS_SPRINTING_ANIMATOR, horizontalInput != 0f);
            }
            else {
                _animator.SetBool(IS_WALKING_ANIMATOR, horizontalInput != 0f);
                _animator.SetBool(IS_SPRINTING_ANIMATOR, false);
            }
            unCrouch();
        }
        
        _animator.SetBool(IS_GROUNDED_ANIMATOR, isOnGround);

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
                Debug.Log("Crouch walking ");
                 _animator.SetBool(IS_CROUCHING_WALKING_ANIMATOR, true);
                _animator.SetBool(IS_CROUCHING_STATIC_ANIMATOR, false); 
            }
            else {
                Debug.Log("Crouch");
                _animator.SetBool(IS_CROUCHING_STATIC_ANIMATOR, true);  
                _animator.SetBool(IS_CROUCHING_WALKING_ANIMATOR, false); 
            }
        }
        else {
            _animator.SetBool(IS_CROUCHING_STATIC_ANIMATOR, false); 
            _animator.SetBool(IS_CROUCHING_WALKING_ANIMATOR, false); 
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
                HandleCrouch();
                break;
            case "Push":
                HandlePush();
                break;
            default:
                ResetPlayer();
                break;
        }
    }

    private void KeyUpHandler(string action)
    {
        switch (action)
        {
            case "Sprint":
                StopSprint();
                break;
            case "Crouch":
                unCrouch();
                break;
            // default:
            //     ResetPlayer();
            //     break;
        }
    }
    // -------------------- action functions -------------------- // 

    private void ResetPlayer()
    {
        walkSpeed = 5f;
        playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocity.y);
        jumpForce = 25f;
    }

    private void unCrouch() {
        // TODO: prevent player from leaving crouching state while in a tunnel
        // also prevent them from getting rid of crouch ability in that case
        playerCollider.offset = initColliderOffset;
        playerCollider.size = initColliderSize;
        isCrouching = false;
    }


    private void HandleSprint()
    {
        Debug.Log("SPROONT");
        isSprinting = true;
    }
    private void StopSprint() {
        Debug.Log("STOOP SPROONT");
        isSprinting = false;
    }

 
    private void HandleJump()
    {
        //if (isOnGround)
        if(IsGrounded()) 
        {
            Jump();
        }
    }
    private void HandleCrouch()
    {
        isCrouching = true;
        playerCollider.offset = new Vector2(initColliderOffset.x, initColliderOffset.y - initColliderSize.y / 4);
        playerCollider.size = new Vector2(initColliderSize.x, initColliderSize.y / 2);

    }

    private void HandlePush() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up + (facingRight ? Vector3.right : Vector3.left), 
        (facingRight ? Vector2.right : Vector2.left) * 2, 3); 
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
        audioSource.PlayOneShot(jumpAudio);
    }
    public void LandOnGround() {
        audioSource.PlayOneShot(landAudio);
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
