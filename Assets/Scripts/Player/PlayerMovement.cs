
using System.Collections;
using UnityEngine;



public partial class PlayerMovement : MonoBehaviour
{


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

    public bool IsRecoiling { get; private set; }
    

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


    [SerializeField] private PlayerMovementData _data;

    private PlayerAnimation _playerAnimation;
    private Rigidbody2D _rb;

    private int _lastWallJumpDirection;
    private float _wallJumpStartTime;


    private int _dashLeft;
    private bool _isDashCooldown;

    [SerializeField] private PlayerDashFX _dashFX;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        SetGravityScale(_data.gravityScale);
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
            LastPressJumpTime = _data.jumpInputBufferTime;
        }


        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame())
        {
            if (CanJumpCut()|| CanWallJumpCut())
            {
                IsJumpCut = true;
                IsJumping = false;
                IsWallJumping = false;
            }
            
            

        }
        if (GameInputManager.Instance.PlayerRunAction.WasPressedThisFrame())
        {
            LastPressDashTime = _data.dashInputBufferTime;
        }
        #endregion

        

        #region Wall Check & Ground Check

        if(!IsDashing )
        {

            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, GroundLayerMask) && !IsJumping)
            {
               LastOnGroundTime = _data.coyoteTime;
            }


            if (!IsWallJumping)
            {
                if ((CheckWall(_rightWallCheckPoint.position) && IsFacingRight) ||(CheckWall(_leftWallCheckPoint.position) && !IsFacingRight))
                {
                   LastOnRightWallTime = _data.coyoteTime;
                }

                if ((CheckWall(_rightWallCheckPoint.position) && !IsFacingRight) ||(CheckWall(_leftWallCheckPoint.position) && IsFacingRight))
                {
                    LastOnLeftWallTime = _data.coyoteTime;
                }


               LastOnWallTime = Mathf.Max(LastOnLeftWallTime,LastOnRightWallTime);
            }
        }


        #endregion

        #region Fall Check
        if(!IsJumping && !IsWallJumping && _rb.linearVelocityY < 0)
        {
            IsFalling = true;
        }



        if (!IsJumping && !IsWallJumping &&LastOnGroundTime > 0 )
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

        if (IsWallJumping && Time.time - _wallJumpStartTime > _data.wallJumpTime)
        {
            IsWallJumping = false;
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
              

                ExecuteJump();


            }
            else if (CanWallJump() && LastPressJumpTime > 0)
                {
                    IsWallJumping = true;
                    IsJumping = false;
                    IsJumpCut = false;
          
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

        if (CanMove())
        {
           
            HandleMove();
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

        float lerpAmount = IsWallJumping ? _data.wallJumpMoveLerp : 1;
        
        float targetSpeed = HorizontalInput * _data.maxMoveSpeed;
        targetSpeed = Mathf.Lerp(_rb.linearVelocityX, targetSpeed, lerpAmount);


        _rb.linearVelocity = new Vector2(targetSpeed, _rb.linearVelocityY);
    }

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    private bool CanJumpCut()
    {
        return IsJumping && _rb.linearVelocityY > 0;
    }
    private bool CanWallJumpCut()
    {
        return IsWallJumping && _rb.linearVelocityY > 0;
    }

    private void ExecuteJump()
    {
        LastPressJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump

        float force = _data.jumpForce;

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
        Vector2 force = new Vector2(_data.wallJumpForce.x, _data.wallJumpForce.y);

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


        
        _rb.linearVelocity = new Vector2(_rb.linearVelocityX, -_data.slideSpeed);

    }
    public void HandlePogo()
    {

        IsJumpCut = false;
        RefillDash();
        if(_rb.linearVelocityY < 0)
        {
            _rb.AddForce(-_rb.linearVelocityY * Vector2.up, ForceMode2D.Impulse);
        }
       

        _rb.AddForce(Vector2.up * _data.pogoForce, ForceMode2D.Impulse);
    }
    public void HandleStopJump()
    {
        IsJumpCut = false;
        if (_rb.linearVelocityY > 0)
        {
            _rb.AddForce(-_rb.linearVelocityY * Vector2.up, ForceMode2D.Impulse);
        }
    }

    private bool CanDash()
    {
        if (!IsDashing && _dashLeft < _data.dashAmount && LastOnGroundTime > 0 && !_isDashCooldown)
        {
            StartCoroutine(StartRefillDash());
        }


        return _dashLeft > 0;
    }

    private void RefillDash()
    {
        _dashLeft = Mathf.Min(_data.dashAmount, _dashLeft + 1);
    }
    private bool CanMove()
    {
        return !IsDashing && !IsRecoiling;
    }
    private IEnumerator StartDash(Vector2 dashDir)
    {
        LastPressDashTime = 0;
        _dashFX.PlayDashFX(true);

        float startTime = Time.time;
        _dashLeft--;
        IsDashing = true;

        while (Time.time - startTime < _data.dashTime)
        {
            _rb.linearVelocity = dashDir.normalized *_data.dashSpeed;

            yield return null;
        }

        IsDashing = false;
        _dashFX.PlayDashFX(false);
    }

    private IEnumerator StartRefillDash()
    {
        _isDashCooldown = true;
        yield return new WaitForSeconds(_data.dashCooldownTime);
        _isDashCooldown = false;
        RefillDash();
    }


    public IEnumerator StartRecoil(Vector2 direction)
    {
        IsRecoiling = true;
        _rb.AddForce(-_rb.linearVelocityX * Vector2.right, ForceMode2D.Impulse);

        _rb.AddForce(direction * _data.recoilForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_data.recoilDuration);
        IsRecoiling = false;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.DrawWireCube(_leftWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_rightWallCheckPoint.position, _wallCheckSize);
    }




}
