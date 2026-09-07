using System;
using UnityEngine;


public partial class Player : MonoBehaviour
{
    public const string PLAYER_TAG = "Player";
    public const string PLAYER_MASK = "Player";

    [Header("Player Velocity")]
    public PlayerVelocity WalkVelocity;
    public PlayerVelocity RunVelocity;
    public PlayerVelocity CrouchWalkVelocity;
    public PlayerVelocity CurrentVelocity;

    [Header("Player Input")]
    public float HorizontalInput;
    public float VerticalInput;
    public int LastHorizontalInput  = 1;

    [Header("Play Stats")]
    public int Damage = 1;
    public float InvincibilityDuration =1.5f;


    [Header("Player State")]

    public bool IsFacingRight = true;
    public int FacingDirection = 1;
    public bool IsGrounded;
    public bool IsFall;
    public bool IsClimbing;
    public bool IsWallSliding;
    public bool IsWallJumping;
    public bool IsCrouching;
    public bool IsCrouchWalking;
    public bool IsRunning;
    public bool IsTurning;
    public bool IsDashing;
    public bool IsAttacking;
    public bool IsInvincible;

    public bool IsHorizontalMoving => Mathf.Abs(HorizontalInput) > 0.1f;
    public bool IsVerticalMoving => Mathf.Abs(VerticalInput) > 0.1f;
    public bool IsStopAction => (IsClimbing || IsDashing || IsTurning);
    public bool IsLeftMove => HorizontalInput < 0f;
    public bool IsRightMove => HorizontalInput > 0f;

    [Header("Gravity")]  
    [SerializeField] private  float _fallGravityMuliplier = 3.5f;
    [SerializeField]  private float _lowJumpMultiplier = 2f;

    [Header("Mask")]
    public LayerMask GroundLayerMask;
    public LayerMask WallLayerMask;
    public LayerMask EnemyLayerMask;

    public PlayerAnimation PlayerAnimation { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public PlayerSprite PlayerSprite { get; private set; }
    private PlayerData _playerData;
    private PlayerJumping _playerJumping;
    private PlayerMoving _playerMoving;
    private PlayerWallJumping _playerWallJumping;
    private PlayerCrouching _playerCrouching;
    private PlayerDashing _playerDashing;
    private PlayerClimbing _playerClimbing;
    private PlayerAttack _playerAttack;

    public int Coin { get => _playerData.Coin;  set => _playerData.Coin = value; }
    public int Health { get => _playerData.Health;  set => _playerData.Health = value; }
  
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
        PlayerSprite = GetComponent<PlayerSprite>();

        _playerJumping = GetComponent<PlayerJumping>();
        _playerMoving = GetComponent<PlayerMoving>();
        _playerWallJumping = GetComponent<PlayerWallJumping>();
        _playerCrouching = GetComponent<PlayerCrouching>();
        _playerDashing = GetComponent<PlayerDashing>();
        _playerClimbing = GetComponent<PlayerClimbing>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start()
    {
        _playerData = GameManager.Instance.PlayerData;
    }
    private void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;

        UpdateDirection();
        UpdateInput();

        _playerClimbing.HandleClimbing();
        _playerAttack.HandleAttack();
        _playerDashing.HandleDashing();
        _playerCrouching.HandleCrouching();
        _playerJumping.HandleJump();
        _playerWallJumping.HandleWallSlide();
        _playerWallJumping.HandleWallJump();
        _playerMoving.HandleMoving();
    }
    private void LateUpdate()
    {
        PlayerSprite.UpdateSprite();
        PlayerAnimation.UpdateAnimation();
        HandleGravity();
    }
    private void FixedUpdate()
    {
        _playerJumping.CheckGround();
        _playerWallJumping.CheckWall();
        _playerClimbing.CheckLedge();
    }
    private void UpdateDirection()
    {
        if (IsRightMove)
        {
            LastHorizontalInput = 1;
        }else if (IsLeftMove)
        {
            LastHorizontalInput = -1;
        }
    }
    private void UpdateInput()
    {
       HorizontalInput = GameInputManager.Instance.GetHorizontalInput();
        VerticalInput= GameInputManager.Instance.GetVerticalInput();

    }

    private void HandleGravity()
    {
       if(Rb.linearVelocityY < 0)
        {
            Rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (_fallGravityMuliplier - 1) * Time.deltaTime;
        }else if(Rb.linearVelocityY >0 && !GameInputManager.Instance.PlayerJumpAction.IsPressed())
        {
            Rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }
    
    public void SetPosition(Vector2 position)
    {
        transform.position= position; 
    }

   
}
