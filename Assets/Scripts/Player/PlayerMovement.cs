
using System.Collections;
using UnityEngine;



public partial class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; }
    public PlayerHealth PHealth => PlayerHealth.Instance;
    public PlayerAnimation PAnimation => PlayerAnimation.Instance;

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
    [SerializeField] private PlayerMovementSO _dataSO;
    [SerializeField] private bool _canDashInAir;
    private Rigidbody2D _rb;
    private BuffEffectManager _buffEffectManager;
    private bool _isDashCooldown;
    private bool _canSprintAfterDash;

    [SerializeField] private DashFX _dashFX;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _buffEffectManager = GetComponent<BuffEffectManager>();
       
    }

    private void Start()
    {
        SetGravityScale(_dataSO.gravityScale);
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
            _lastPressJumpTimer = _dataSO.jumpInputBufferTime;
        }
        if (GameInputManager.Instance.PlayerActions.Jump.WasReleasedThisFrame())
        {
            if (CanJumpCut())
            {
                IsJumpCut = true;
                IsJumping = false;

            }



        }

        if (GameInputManager.Instance.PlayerActions.Sprint.WasPressedThisFrame())
        {
            _lastPressDashTimer = _dataSO.dashInputBufferTime;
        }
        if (GameInputManager.Instance.PlayerActions.Sprint.WasReleasedThisFrame())
        {
            _canSprintAfterDash = false;
        }
        #endregion
        #region  Ground Check
        if (!IsDashing)
        {

            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, GroundLayerMask) && !IsJumping)
            {
                _lastOnGroundTimer = _dataSO.coyoteTime;
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
        if (CanDash())
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
    public float GetDistanceFromPosition(Vector3 position)
    {
        return Vector2.Distance(transform.position, position);
    }


    public float GetFallHeight()
    {
        RaycastHit2D hit = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, Mathf.Infinity, GroundLayerMask);

        if (hit.collider != null)
        {
            return hit.distance;
        }
        else
        {
            return 0f;
        }
    }
    private void HandleSprint()
    {
        if (CanSprint())
        {
            IsSprinting = true;
            
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
       
        PAnimation.UpdateAnimation();
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
        }
      


       
    }

    

    

    private void HandleGravity()
    {
        if ( IsDashing )
        {
            
            SetGravityScale(0);
        }else if (IsPlunging)
        {
            SetGravityScale(_dataSO.gravityScale * _dataSO.plungingGravityMult);
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -_dataSO.maxFallSpeed));
        }
        else if (IsJumpCut)
        {
           
            SetGravityScale(_dataSO.gravityScale * _dataSO.jumpCutGravityMult);
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -_dataSO.maxFallSpeed));
        }
        else if( _rb.linearVelocityY < 0)
        {
           
            //Higher gravity if falling
            SetGravityScale(_dataSO.gravityScale * _dataSO.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -_dataSO.maxFallSpeed));
        }
        else
        {
            SetGravityScale(_dataSO.gravityScale);
        }

    }


    private void HandleMove()
    {
        
        float moveSpeed= IsSprinting ? _dataSO.maxSprintSpeed: _dataSO.maxMoveSpeed;

        moveSpeed *= GetMoveSpeedMultiplier();
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

        float force = _dataSO.jumpForce;

        if (_rb.linearVelocityY < 0)
        {
            force -= _rb.linearVelocityY;
        }
       _rb.AddForce(force * Vector2.up, ForceMode2D.Impulse);
        #endregion
    }

    private bool CanDash()
    {
        if(_lastPressDashTimer > 0f && !IsDashing && !_isDashCooldown)
        {

            if (!_canDashInAir) return IsGrounded;
            return true;
        }

        return false;
            
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
        PHealth.IsInvincible = true;

      

        while (Time.time - startTime < _dataSO.dashTime)
        {
            _rb.linearVelocity = dashDir.normalized *_dataSO.dashSpeed;

            yield return null;
        }

        IsDashing = false;
        PHealth.IsInvincible = false;
        _canSprintAfterDash = true;
        _dashFX.PlayDashFX(false);

        
        yield return new WaitForSeconds(_dataSO.dashCooldownTime);
        _isDashCooldown = false;
    }



    private bool CanSprint()
    {

        return GameInputManager.Instance.PlayerActions.Sprint.IsPressed() && IsGrounded
            && IsHorizontalMoving && _canSprintAfterDash;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }
    private float GetMoveSpeedMultiplier()
    {
        float multiplier = 1;
        if (_buffEffectManager.HasEffect(BuffEffectType.MoveSpeedBonus))
        {
            multiplier = (1 + _buffEffectManager.GetValues(BuffEffectType.MoveSpeedBonus));
        }

        return multiplier;
    }
}
