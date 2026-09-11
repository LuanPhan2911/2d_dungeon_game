
using System.Collections;
using UnityEngine;



public partial class PlayerMovement : MonoBehaviour
{
    public PlayerMovementData Data;

    [Header("Player Input")]
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    public int LastHorizontalInput { get; private set; }

    [Header("Player State")]
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsJumpCut { get; private set; }
    public bool IsFalling { get; private set; }
    public bool IsWallSliding { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }


    public bool IsHorizontalMoving => Mathf.Abs(HorizontalInput) > 0.1f;
    public bool IsVerticalMoving => Mathf.Abs(VerticalInput) > 0.1f;
    public bool IsLeftMove => HorizontalInput < 0f;
    public bool IsRightMove => HorizontalInput > 0f;

    public float LastOnLeftWallTime { get; private set; }
    public float LastOnRightWallTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnGroundTime { get; private set; }
    public float LastPressJumpTime { get; private set; }
    public float LastPressDashTime { get; private set; }


    [Header("Ground Mask & Wall Mask")]
    public LayerMask GroundLayerMask;
    public LayerMask WallLayerMask;
    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.3f, 0.03f);
    [Header("Wall Check")]
    [SerializeField] private Transform _leftWallCheckPoint;
    [SerializeField] private Transform _rightWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.03f, 0.3f);

    private PlayerAnimation _playerAnimation;
    private Rigidbody2D _rb;

    private int _lastWallJumpDirection;
    private float _wallJumpStartTime;


    private int _dashLeft;
    private bool _isDashCooldown;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        SetGravityScale(Data.gravityScale);
        IsFacingRight = true;
        LastHorizontalInput = 1;
    }
    private void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;

        #region Timer
        LastPressJumpTime -= Time.deltaTime;
        LastOnGroundTime -= Time.deltaTime;

        LastOnLeftWallTime-= Time.deltaTime;
        LastOnRightWallTime-= Time.deltaTime;
        LastOnWallTime-= Time.deltaTime;
        LastPressDashTime -= Time.deltaTime;

        #endregion

       

        #region Input Handler
        HandleInput();
        CheckDirectionToFace();

        if (GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame())
        {
            LastPressJumpTime = Data.jumpInputBufferTime;
        }


        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame())
        {
            if (CanJumpCut())
            {
                IsJumpCut = true;
            }

        }
        if (GameInputManager.Instance.PlayerRunAction.WasPressedThisFrame())
        {
            LastPressDashTime = Data.dashInputBufferTime;
        }
        #endregion

        

        #region Wall Check & Ground Check

        if(!IsDashing && !IsJumping)
        {

            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, GroundLayerMask))
            {
               LastOnGroundTime = Data.coyoteTime;
            }


            if (!IsWallJumping)
            {
                if ((CheckWall(_rightWallCheckPoint.position) && IsFacingRight) ||(CheckWall(_leftWallCheckPoint.position) && !IsFacingRight))
                {
                   LastOnRightWallTime = Data.coyoteTime;
                }

                if ((CheckWall(_rightWallCheckPoint.position) && !IsFacingRight) ||(CheckWall(_leftWallCheckPoint.position) && IsFacingRight))
                {
                    LastOnLeftWallTime = Data.coyoteTime;
                }


               LastOnWallTime = Mathf.Max(LastOnLeftWallTime,LastOnRightWallTime);
            }
        }


        #endregion

        #region Jump Check

        if (IsJumping && _rb.linearVelocityY < 0)
        {
            IsJumping = false;
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
            IsFalling = true;
        }


        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            IsJumpCut = false;
            IsFalling = false;
        }

        #endregion


        #region Handle Jump


        if (!IsDashing)
        {
            if (CanJump() && LastPressJumpTime > 0)
            {
               IsJumping = true;
               IsWallJumping = false;
               IsJumpCut = false;
               IsFalling = false;

                ExecuteJump();


            }
            else if (CanWallJump() && LastPressJumpTime > 0)
                {
                    IsWallJumping = true;
                    IsJumping = false;
                    IsJumpCut = false;
                    IsFalling = false;

                    _wallJumpStartTime = Time.time;

                    _lastWallJumpDirection = LastOnRightWallTime > 0 ? -1 : 1;

                    ExecuteWallJump();
                }

           
        }

        #endregion





        #region Handle Dash

        if (CanDash() && LastPressDashTime > 0)
        {
            Vector2 dashDir;
            if (IsHorizontalMoving && !IsWallSliding)
            {
                dashDir = HorizontalInput * Vector2.right;
            }
            else
            {
                dashDir = IsFacingRight ? Vector2.right : Vector2.left;
            }

            IsJumping = false;
            IsWallJumping = false;
            IsJumpCut = false;
            StartCoroutine(StartDash(dashDir));
        }
        #endregion

        #region Wall Slide Check
        if (CanWallSlide() &&
           ((LastOnLeftWallTime > 0 && IsLeftMove) || (LastOnRightWallTime > 0 && IsRightMove)))
        {

            IsWallSliding = true;
            IsJumpCut = false;
        }
        else
        {
            IsWallSliding = false;
        }

        #endregion
    }
    private void LateUpdate()
    {
       
        _playerAnimation.UpdateAnimation();
        HandleGravity();
    }
    private void FixedUpdate()
    {

        if (!IsDashing)
        {
            if (IsWallJumping)
            {
                HandleRun(Data.wallJumpRunLerp);
            }
            else
            {
                HandleRun();
            }
        }


        if (IsWallSliding)
        {
           HandleWallSlide();
        }
       

    }
    private bool CheckWall(Vector2 position)
    {
        return Physics2D.OverlapBox(position, _wallCheckSize, 0f, WallLayerMask);
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
        if (IsRightMove)
        {
            LastHorizontalInput = 1;
        }
        else if (IsLeftMove)
        {
            LastHorizontalInput = -1;
        }

        if (IsWallSliding)
        {
            Turn(-LastHorizontalInput);
        }
        else
        {
            Turn(LastHorizontalInput);
        }
       
       
      
        
    }


    private void Turn(int facingDirection)
    {
        if (IsDashing) return;


        Vector3 scale = transform.localScale;
        scale.x = facingDirection;
        transform.localScale = scale;
        IsFacingRight = facingDirection==1;
    }

    private void HandleGravity()
    {
        if (IsWallSliding|| IsDashing)
        {
            SetGravityScale(0);
        }
        else if (IsJumpCut)
        {
            IsFalling = true;
            SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -Data.maxFallSpeed));
        }
        else if((IsJumping || IsWallJumping || IsFalling) && Mathf.Abs(_rb.linearVelocityY) < Data.jumpHangTimeThreshold)
        {
            SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
        }else if(_rb.linearVelocityY < 0)
        {
            IsFalling = true;
            //Higher gravity if falling
            SetGravityScale(Data.gravityScale * Data.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, Mathf.Max(_rb.linearVelocityY, -Data.maxFallSpeed));
        }
        else
        {
            SetGravityScale(Data.gravityScale);
        }

    }


    private void HandleRun(float lerpAmount = 1)
    {
       

        
        float targetSpeed = HorizontalInput * Data.runMaxSpeed;

        targetSpeed = Mathf.Lerp(_rb.linearVelocityX, targetSpeed, lerpAmount);
        float accelRate;
        if (!IsJumping)
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        }
        else
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir :
                Data.runDeccelAmount * Data.deccelInAir;
        }

        if ((IsJumping || IsWallJumping || IsFalling) &&
            Mathf.Abs(_rb.linearVelocityY) < Data.jumpHangTimeThreshold)
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }



        float speedDiff = targetSpeed - _rb.linearVelocityX;

        float movement = speedDiff * accelRate;

        _rb.AddForce(movement * Vector2.right, ForceMode2D.Force);






    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    private bool CanJumpCut()
    {
        return IsJumping && _rb.linearVelocityY > 0;
    }

    private void ExecuteJump()
    {
        LastPressJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump

        float force = Data.jumpForce;

        if (_rb.linearVelocityY < 0)
        {
            force -= _rb.linearVelocityY;
        }
       _rb.AddForce(force * Vector2.up, ForceMode2D.Impulse);
        #endregion
    }


    private bool CanWallSlide()
    {
        return LastOnWallTime > 0 && LastOnGroundTime <= 0 &&!IsJumping && !IsWallJumping && !IsDashing;
    }


    private bool CanWallJump()
    {
        return LastOnWallTime > 0 && LastOnGroundTime <= 0 &&!IsWallJumping;
    }

    private void ExecuteWallJump()
    {
        LastOnRightWallTime = 0;
        LastPressJumpTime = 0;
        LastOnLeftWallTime = 0;
       LastOnRightWallTime = 0;

        #region Perform Wall Jump
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);

        force.x *= _lastWallJumpDirection;

        if (Mathf.Sign(_rb.linearVelocityX) != Mathf.Sign(force.x))
        {
            force.x -= _rb.linearVelocityX;
        }
        if (_rb.linearVelocityY < 0)
        {
            force.y -= _rb.linearVelocityY;
        }

        _rb.AddForce(force, ForceMode2D.Impulse);

        #endregion
    }
   


    private void HandleWallSlide()
    {

        if (_rb.linearVelocityY > 0)
        {
            _rb.AddForce(-_rb.linearVelocityY * Vector2.up, ForceMode2D.Impulse);
        }

        float speedDiff = Data.slideSpeed - _rb.linearVelocityY;

        float movement = speedDiff * Data.slideAccel;

        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDiff) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDiff) * (1 / Time.fixedDeltaTime));

        _rb.AddForce(movement * Vector2.up, ForceMode2D.Force);

    }

    private bool CanDash()
    {
        if (!IsDashing && _dashLeft < Data.dashAmount && LastOnGroundTime > 0 && !_isDashCooldown)
        {
            StartCoroutine(RefillDash());
        }


        return _dashLeft > 0;
    }
    private IEnumerator StartDash(Vector2 dashDir)
    {
        LastPressDashTime = 0;

        float startTime = Time.time;
        _dashLeft--;
        IsDashing = true;

        while (Time.time - startTime < Data.dashTime)
        {
            _rb.linearVelocity = dashDir.normalized *Data.dashSpeed;

            yield return null;
        }

        IsDashing = false;
    }

    private IEnumerator RefillDash()
    {
        _isDashCooldown = true;
        yield return new WaitForSeconds(Data.dashCooldownTime);
        _isDashCooldown = false;
        _dashLeft = Mathf.Min(Data.dashAmount, _dashLeft + 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);


        Gizmos.color = Color.blue;


        Gizmos.DrawWireCube(_leftWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_rightWallCheckPoint.position, _wallCheckSize);
    }




}
