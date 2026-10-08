
using System.Collections;
using UnityEngine;



public partial class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; }

    [Header("Player Input")]
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }


    [Header("Player State")]
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsJumpCut { get; private set; }
    public bool IsFalling { get; private set; }
    public bool IsDashing { get; private set; }

    public bool IsSprinting { get; private set; }

    [HideInInspector]
    public bool IsPlunging;
 

    public float GetFallHeight()
    {
        RaycastHit2D hit= Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, Mathf.Infinity, GroundLayerMask);

        if (hit.collider != null)
        {
            return hit.distance;
        }
        else
        {
            return 0f;
        }
    }
    public float GetDistanceFromPosition(Vector3 position)
    {
        return Vector2.Distance(transform.position, position);
    }
    public bool IsGrounded => _lastOnGroundTimer > 0f;






    public bool IsHorizontalMoving => HorizontalInput != 0f;
    public bool IsLeftMove => HorizontalInput < 0f;
    public bool IsRightMove => HorizontalInput > 0f;


    private float _lastOnGroundTimer;
    private float _lastPressJumpTimer;
    private float _lastPressDashTimer;

  




    [Header("Ground Mask")]
    public LayerMask GroundLayerMask;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.3f, 0.03f);




    [SerializeField] private PlayerMovementSO _data;

    [SerializeField] private Transform _UITransform;

    private PlayerAnimation _playerAnimation;
    private Rigidbody2D _rb;




    private bool _isDashCooldown;
    private bool _canSprintAfterDash;

    [SerializeField] private DashFX _dashFX;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
       
    }

    private void Start()
    {
        SetGravityScale(_data.gravityScale);
        IsFacingRight = true;
        
    }
    private void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;

        #region Timer
        _lastPressJumpTimer = Mathf.Max(0, _lastPressJumpTimer - Time.deltaTime);
        _lastOnGroundTimer = Mathf.Max(0, _lastOnGroundTimer - Time.deltaTime);
        _lastPressDashTimer = Mathf.Max(0, _lastPressDashTimer - Time.deltaTime);

        #endregion



        #region Input Handler
        HandleInput();
        CheckDirectionToFace();

        if (GameInputManager.Instance.PlayerActions.Jump.WasPressedThisFrame())
        {
            _lastPressJumpTimer = _data.jumpInputBufferTime;
        }
        if (GameInputManager.Instance.PlayerActions.Jump.WasReleasedThisFrame())
        {
            if (CanJumpCut())
            {
                IsJumpCut = true;
                IsJumping = false;

            }



        }

        if (GameInputManager.Instance.PlayerActions.Run.WasPressedThisFrame())
        {
            _lastPressDashTimer = _data.dashInputBufferTime;
        }
        if (GameInputManager.Instance.PlayerActions.Run.WasReleasedThisFrame())
        {
            _canSprintAfterDash = false;
        }
        #endregion
        #region  Ground Check
        if (!IsDashing)
        {

            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, GroundLayerMask) && !IsJumping)
            {
                _lastOnGroundTimer = _data.coyoteTime;
            }

        }
        #endregion

        #region Fall Check
        if (!IsJumping && _rb.linearVelocityY < 0)
        {
            IsFalling = true;
        }
        if (!IsJumping && IsGrounded)
        {
            IsJumpCut = false;
            IsFalling = false;
        }
        #endregion

        #region Jump Check

        if (IsJumping && _rb.linearVelocityY < 0)
        {
            IsJumping = false;
        }
        #endregion


        #region Handle Jump
        HandleJump();
        #endregion

        #region Handle Dash & Sprint
        HandleDash();

        HandleSprint();
        #endregion


    }

    private void HandleDash()
    {
        if (CanDash() && _lastPressDashTimer > 0)
        {
            Vector2 dashDir;

            if (IsHorizontalMoving)
            {
                dashDir = HorizontalInput * Vector2.right;
            }
            else
            {
                dashDir = IsFacingRight ? Vector2.right : Vector2.left;
            }

            IsJumping = false;
            IsJumpCut = false;
            StartCoroutine(StartDash(dashDir));
        }
       
    }
    private void HandleSprint()
    {
        if (CanSprint())
        {
            IsSprinting = true;
            PlayerHealth.Instance.ConsumeStamina(_data.sprintStaminaCostPerSecond * Time.deltaTime);
        }
        else
        {
            _canSprintAfterDash = false;
            IsSprinting = false;
        }
    }

    private void HandleJump()
    {
        if (!IsDashing)
        {
            if (CanJump() && _lastPressJumpTimer > 0)
            {
                IsJumping = true;
                IsJumpCut = false;
                ExecuteJump();
            }
        }
    }

    private void LateUpdate()
    {
       
        _playerAnimation.UpdateAnimation();
        HandleGravity();
    }
    private void FixedUpdate()
    {

        if (CanMove())
        {
           
            HandleMove();
        }
    }
   
    private void SetGravityScale(float gravityScale)
    {
        _rb.gravityScale=gravityScale;
    }


    private void HandleInput()
    {
        HorizontalInput = GameInputManager.Instance.GetHorizontalInput();
        VerticalInput = GameInputManager.Instance.GetVerticalInput();

    }
    private void CheckDirectionToFace()
    {
        if (IsDashing) return;

        if ((IsRightMove && !IsFacingRight)||(IsLeftMove && IsFacingRight))
        {
            IsFacingRight = !IsFacingRight;


            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            Vector3 UIScale = _UITransform.localScale;
            UIScale.x *= -1;
            _UITransform.localScale = UIScale;
        }
      


       
    }

    

    

    private void HandleGravity()
    {
        if ( IsDashing )
        {
            
            SetGravityScale(0);
        }else if (IsPlunging)
        {
            SetGravityScale(_data.gravityScale * _data.plungingGravityMult);
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -_data.maxFallSpeed));
        }
        else if (IsJumpCut)
        {
           
            SetGravityScale(_data.gravityScale * _data.jumpCutGravityMult);
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -_data.maxFallSpeed));
        }
        else if( _rb.linearVelocityY < 0)
        {
           
            //Higher gravity if falling
            SetGravityScale(_data.gravityScale * _data.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -_data.maxFallSpeed));
        }
        else
        {
            SetGravityScale(_data.gravityScale);
        }

    }


    private void HandleMove()
    {
        
        float moveSpeed= IsSprinting ? _data.maxSprintSpeed: _data.maxMoveSpeed;
        float targetSpeed = HorizontalInput * moveSpeed;
       
        _rb.linearVelocity = new Vector2(targetSpeed, _rb.linearVelocityY);
    }

    private bool CanJump()
    {
        return IsGrounded && !IsJumping;
    }
    private bool CanJumpCut()
    {
        return IsJumping && _rb.linearVelocityY > 0;
    }
 

    private void ExecuteJump()
    {
        _lastPressJumpTimer = 0;
        _lastOnGroundTimer = 0;

        #region Perform Jump

        float force = _data.jumpForce;

        if (_rb.linearVelocityY < 0)
        {
            force -= _rb.linearVelocityY;
        }
       _rb.AddForce(force * Vector2.up, ForceMode2D.Impulse);
        #endregion
    }

    private bool CanDash()
    {
        return !IsDashing && IsGrounded && !_isDashCooldown &&
            PlayerHealth.Instance.CurrentStamina >= _data.staminaCostPerDash;
    }

   
    private bool CanMove()
    {
        return !IsDashing && !IsPlunging ;
    }
    private IEnumerator StartDash(Vector2 dashDir)
    {
        _lastPressDashTimer = 0;
        _isDashCooldown = true;
        _dashFX.PlayDashFX(true);

        float startTime = Time.time;
        IsDashing = true;

        PlayerHealth.Instance.ConsumeStamina(_data.staminaCostPerDash);

        while (Time.time - startTime < _data.dashTime)
        {
            _rb.linearVelocity = dashDir.normalized *_data.dashSpeed;

            yield return null;
        }

        IsDashing = false;
        _canSprintAfterDash = true;
        _dashFX.PlayDashFX(false);

        
        yield return new WaitForSeconds(_data.dashCooldownTime);
        _isDashCooldown = false;
    }



    private bool CanSprint()
    {

        return GameInputManager.Instance.PlayerActions.Run.IsPressed() && IsGrounded
            && IsHorizontalMoving && _canSprintAfterDash
            && PlayerHealth.Instance.CurrentStamina >= _data.sprintStaminaCostPerSecond * Time.deltaTime;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }
}
