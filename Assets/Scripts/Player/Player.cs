using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


public partial class Player : MonoBehaviour
{


    [Header("Player Velocity")]
    public PlayerVelocity WalkVelocity;
    public PlayerVelocity RunVelocity;
    public PlayerVelocity CrouchWalkVelocity;
    public PlayerVelocity CurrentVelocity;

    public PlayerMovementData Data;

    [Header("Player Input")]
    public float HorizontalInput;
    public float VerticalInput;
    public int LastHorizontalInput = 1;

    [Header("Player State")]
    public bool IsFacingRight=true;
    public bool IsJumping;
    public bool IsJumpCut;
    public bool IsFalling;

    public bool IsWallSliding;
    public bool IsWallJumping;
    public bool IsCrouching;
    public bool IsCrouchWalking;
    public bool IsTurning;
    public bool IsDashing;
    public bool IsAttacking;

    public bool IsHorizontalMoving => Mathf.Abs(HorizontalInput) > 0.1f;
    public bool IsVerticalMoving => Mathf.Abs(VerticalInput) > 0.1f;
    public bool IsStopAction =>  ( IsDashing || IsTurning);
    public bool IsLeftMove => HorizontalInput < 0f;
    public bool IsRightMove => HorizontalInput > 0f;

    public float LastOnLeftWallTime;
    public float LastOnRightWallTime;
    public float LastOnWallTime;

    public float LastOnGroundTime;
    public float LastPressJumpTime;


    [Header("Mask")]
    public LayerMask GroundLayerMask;
    public LayerMask WallLayerMask;
    public LayerMask EnemyLayerMask;

    public PlayerAnimation PlayerAnimation { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    private PlayerJumping _playerJumping;
    private PlayerMoving _playerMoving;
    private PlayerWallJumping _playerWallJumping;
    private PlayerCrouching _playerCrouching;
    private PlayerDashing _playerDashing;
  
    private PlayerAttack _playerAttack;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
   
        PlayerAnimation = GetComponent<PlayerAnimation>();
       

        _playerJumping = GetComponent<PlayerJumping>();
        _playerMoving = GetComponent<PlayerMoving>();


        _playerWallJumping = GetComponent<PlayerWallJumping>();
        //_playerCrouching = GetComponent<PlayerCrouching>();
        //_playerDashing = GetComponent<PlayerDashing>();
      
        //_playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        SetGravityScale(Data.gravityScale);
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

        #endregion

        CheckDirectionToFace();

        #region Input Handler
        HandleInput();
        #endregion


        _playerJumping.CheckGround();
        _playerWallJumping.CheckWall();
        //_playerAttack.HandleAttack();
        //_playerDashing.HandleDashing();
        //_playerCrouching.HandleCrouching();

        #region Handle Jump

        if (GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame())
        {
           LastPressJumpTime =Data.jumpInputBufferTime;
        }


        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame())
        {
            if (_playerJumping.CanJumpCut()||_playerWallJumping.CanWallJumpCut() )
            {
                IsJumpCut = true;
            }

        }
        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            IsJumpCut = false;
            IsFalling = false;
        }

        _playerJumping.HandleJump();

        _playerWallJumping.HandleWallJump();

        #endregion

        _playerWallJumping.CheckWallSlide();

    }
    private void LateUpdate()
    {
       
        PlayerAnimation.UpdateAnimation();
        HandleGravity();
    }
    private void FixedUpdate()
    {

        if (IsWallJumping)
        {
            _playerMoving.HandleMoving(Data.wallJumpRunLerp);
        }
        else
        {
            _playerMoving.HandleMoving();
        }


        if (IsWallSliding)
        {
            _playerWallJumping.HandleSlide();
        }
       

    }
    private void SetGravityScale(float gravityScale)
    {
        Rb.gravityScale=gravityScale;
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
        Vector3 scale = transform.localScale;
        scale.x = facingDirection;
        transform.localScale = scale;
        IsFacingRight = facingDirection==1;
    }

    private void HandleGravity()
    {
        if (IsWallSliding)
        {
            SetGravityScale(0);
        }
        else if (IsJumpCut)
        {
            SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
        }else if(IsJumping && Mathf.Abs(Rb.linearVelocityY) < Data.jumpHangTimeThreshold)
        {
            SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
        }else if(Rb.linearVelocityY < 0)
        {
            //Higher gravity if falling
            SetGravityScale(Data.gravityScale * Data.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            Rb.linearVelocity = new Vector2(Rb.linearVelocityX, Mathf.Max(Rb.linearVelocityY, -Data.maxFallSpeed));
        }
        else
        {
            SetGravityScale(Data.gravityScale);
        }

    }




}
