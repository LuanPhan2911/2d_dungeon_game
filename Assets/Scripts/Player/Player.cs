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

    public PlayerVelocity DefaultPlayerVelocity;
    public PlayerVelocity CurrentPlayerVelocity;
  


    [Header("Player State")]
    public bool IsGrounded;
    public bool IsSwimming;
    public bool IsFall;
    public bool IsClimbing;
    public bool IsWallSliding;
    public bool IsWallJumping;

 
    public float GravityScale = 1f;
    public int JumpAvailable = 2;

    [Header("Air Jump")]
    public float AirJumpVelocity = 4f;
    public float MaxAirJumpDuration = 0.4f;
    public ParticleSystem AirJumpFX;

    [Header("Mask")]
    public LayerMask GroundLayerMask;
    public LayerMask WaterLayerMask;
    public LayerMask PlatformLayer;




    public PlayerAnimation PlayerAnimation { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public PlayerSprite PlayerSprite { get; private set; }

   
    [SerializeField] private AudioClip _hurtSound;

    private KnockbackReceiver _knockbackReceiver;
    private DamageFlash _damageFlash;
    private PlayerData _playerData;
    private PlayerJumping _playerJumping;
    private PlayerFalling _playerFalling;
    private PlayerMoving _playerMoving;
    private PlayerClimbing _playerClimbing;
    private PlayerWallSliding _playerWallSliding;
    private PlayerWallJumping _playerWallJumping;

    public int Coin { get => _playerData.Coin; private set => _playerData.Coin = value; }
    public int Health { get => _playerData.Health; private set => _playerData.Health = value; }

    public static event EventHandler<int> OnCoinChanged;
    public static event EventHandler<int> OnHealthChanged;
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
        PlayerInput = GetComponent<PlayerInput>();

        PlayerSprite = GetComponent<PlayerSprite>();

        _knockbackReceiver = GetComponent<KnockbackReceiver>();
        _damageFlash = GetComponent<DamageFlash>();
        _playerJumping = GetComponent<PlayerJumping>();
        _playerMoving = GetComponent<PlayerMoving>();
        _playerFalling = GetComponent<PlayerFalling>();
        _playerClimbing = GetComponent<PlayerClimbing>();
        _playerWallSliding = GetComponent<PlayerWallSliding>();
        _playerWallJumping = GetComponent<PlayerWallJumping>();



    }


    private void Start()
    {
        _playerData = GameManager.Instance.PlayerData;

        // Update the UI with the current coin count at the start of the game
        OnCoinChanged?.Invoke(this, Coin);
        OnHealthChanged?.Invoke(this, Health);

       
    }
    private void OnEnable()
    {
        PlayerInput.actions["Pause"].performed += Player_PauseGame;
    }
    private void OnDisable()
    {
        PlayerInput.actions["Pause"].performed -= Player_PauseGame;
    }

    private void Player_PauseGame(InputAction.CallbackContext obj)
    {
      
        if (GameManager.Instance.IsGamePaused)
        {
            GameManager.Instance.Unpause();
        }
        else
        {
            GameManager.Instance.Pause();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;


        HandleVelocity();
        _playerJumping.GroundCheck();
        _playerWallSliding.WallCheck();

        if (_knockbackReceiver.IsKnockbacked) return;
        if (IsFall)
        {
            // stunned player when falling great height
            HorizontalVelocity = 0;
            VerticalVelocity = 0;
            return;
        }
        if (!IsClimbing)
        {
            if (IsGrounded)
            {
                _playerWallJumping.JumpRemaining = 0;
            }else if (IsWallSliding)
            {
                _playerJumping.JumpRemaining = 0;
            }

            _playerWallJumping.HandleWallJump();
            _playerJumping.HandleJump();
           
         

            if (!IsWallJumping)
            {
                _playerMoving.HandleMoving();
            }
        }


        _playerClimbing.HandleClimbing();





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

    public void PlayAirJumpFX()
    {
        if (!AirJumpFX.isPlaying)
        {
            AirJumpFX.Play();
        }
    }
}
