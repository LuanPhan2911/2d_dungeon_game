using System;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public const string PLAYER_TAG = "Player";
    public const string PLAYER_MASK = "Player";

    [Header("Player Velocity")]
    public float HorizontalVelocity;
    public float VerticalVelocity;

    public PlayerVelocity WalkVelocity;
    public PlayerVelocity RunVelocity;

    public PlayerVelocity CrochWalkVelocity;
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
    public bool IsCroching;
    public bool IsCrochWalking;
    public bool IsRunning;


    public float GravityScale = 1f;
  
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
    private PlayerFalling _playerFalling;
    private PlayerMoving _playerMoving;
    private PlayerWallSliding _playerWallSliding;
    private PlayerWallJumping _playerWallJumping;

    private PlayerCroching _playerCroching;

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
        _playerFalling = GetComponent<PlayerFalling>();
        _playerWallSliding = GetComponent<PlayerWallSliding>();
        _playerWallJumping = GetComponent<PlayerWallJumping>();
        _playerCroching = GetComponent<PlayerCroching>();



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

        HandleVelocity();
        _playerCroching.CheckCroch();
        _playerJumping.CheckGround();
        _playerWallSliding.CheckWall();
        _playerMoving.CheckRun();

        if (_knockbackReceiver.IsKnockbacked) return;
        if (IsFall)
        {
            // stunned player when falling great height
            HorizontalVelocity = 0;
            VerticalVelocity = 0;
            return;
        }
       

        _playerWallJumping.HandleWallJump();

        if (!IsCroching)
        {
            _playerJumping.HandleJump();
        }



        if (!IsWallJumping)
        {
            _playerMoving.HandleMoving();
        }
    }
    private void LateUpdate()
    {
        if (!IsWallSliding)
        {
            PlayerSprite.UpdateSprite();
        }

        PlayerAnimation.UpdateAnimation();
    }

    private void HandleVelocity()
    {
        if (IsClimbing)
        {
            HorizontalVelocity = 0;
            Rb.gravityScale = 0f;
            return;
        }

        if (VerticalVelocity >= 0)
        {
            _playerJumping.HandleJumpVelocity();
        }
        else
        {
            _playerFalling.HandleFallingVelocity();
        }

    }

    private void FixedUpdate()
    {

        Rb.linearVelocity = new Vector2(HorizontalVelocity, VerticalVelocity);
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

   
}
