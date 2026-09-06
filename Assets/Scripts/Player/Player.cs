using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public partial class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public const string PLAYER_TAG = "Player";
    public const string PLAYER_MASK = "Player";


    public PlayerVelocity WalkVelocity;
    public PlayerVelocity RunVelocity;

    public PlayerVelocity CrouchWalkVelocity;
    public PlayerVelocity CurrentVelocity;

    [Header("Player Input")]
    public float HorizontalInput;
    public float VerticalInput;

   



    [Header("Player State")]

    public bool IsFacingRight = true;
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



    public bool IsHorizontalMoving => Mathf.Abs(HorizontalInput) > 0.1f;
    public bool IsVerticalMoving => Mathf.Abs(VerticalInput) > 0.1f;
    public int DirectionX = 1;
    public bool IsStopAction => (IsClimbing || IsDashing || IsTurning);


    public bool IsLeftMove => HorizontalInput < 0f;
    public bool IsRightMove => HorizontalInput > 0f;
    [Header("Gravity")]  
    public float FallGravityMuliplier = 3.5f;
    public float LowJumpMultiplier = 2f;
  
    [Header("Mask")]
    public LayerMask GroundLayerMask;
    public LayerMask WaterLayerMask;
    public LayerMask WallLayerMask;

    public PlayerAnimation PlayerAnimation { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public PlayerSprite PlayerSprite { get; private set; }

   
    [SerializeField] private AudioClip _hurtSound;

    private KnockbackReceiver _knockbackReceiver;
    private DamageFlash _damageFlash;
    private PlayerData _playerData;
    private PlayerJumping _playerJumping;
    private PlayerMoving _playerMoving;
    private PlayerWallJumping _playerWallJumping;
    private PlayerCrouching _playerCrouching;
    private PlayerDashing _playerDashing;
    private PlayerClimbing _playerClimbing;
    private PlayerAttack _playerAttack;

    public int Coin { get => _playerData.Coin; private set => _playerData.Coin = value; }
    public int Health { get => _playerData.Health; private set => _playerData.Health = value; }

    public static event EventHandler<int> OnCoinChanged;
    public static event EventHandler<int> OnHealthChanged;


    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
        PlayerSprite = GetComponent<PlayerSprite>();

        _knockbackReceiver = GetComponent<KnockbackReceiver>();
        _damageFlash = GetComponent<DamageFlash>();
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

        // Update the UI with the current coin count at the start of the game
        OnCoinChanged?.Invoke(this, Coin);
        OnHealthChanged?.Invoke(this, Health);

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
            DirectionX = 1;
        }else if (IsLeftMove)
        {
            DirectionX = -1;
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
            Rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (FallGravityMuliplier - 1) * Time.deltaTime;
        }else if(Rb.linearVelocityY >0 && !GameInputManager.Instance.PlayerJumpAction.IsPressed())
        {
            Rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
        }

    }
    

  
    public void AddCoin()
    {
        Coin++;
        OnCoinChanged?.Invoke(this, Coin);
    }
    public void TakeDamage()
    {
        Health--;
        AudioManager.Instance.Play(_hurtSound, transform.position);
        OnHealthChanged?.Invoke(this, Health);
        _damageFlash.Flash();
        if (Health <= 0)
        {
            // Handle player death (e.g., reload the scene, show game over screen, etc.)

            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);

        }
    }
    public void TakeKnockback(Vector2 direction)
    {
        _knockbackReceiver.Knockback(direction);

    }
    public void Bounce(Vector2 normal, float force)
    {
        Rb.linearVelocity = Vector2.zero;

        Rb.AddForce(normal * force, ForceMode2D.Impulse);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position= position; 
    }

   
}
