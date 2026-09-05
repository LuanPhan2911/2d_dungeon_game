using System.Diagnostics;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{

    [SerializeField] private float _jumpVelocityY = 8f;

    [SerializeField] private Vector2 _runJumpVelocity = new Vector2(10, 10);

    [SerializeField] private AudioClip _jumpSound;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(1, 0.2f);


    [Header("Juice Mechanics")]
    // Grace period to jump after walking off a ledge(in seconds)
    [SerializeField]  private float _coyoteTime = 0.15f;

    [SerializeField] private float _jumpBufferTime = 0.15f; 
    private float _coyoteTimeCounter;

      // How early a player can press jump before landing (in seconds)
    private float _jumpBufferCounter;



    private Player _player;



    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_groundCheckTransform.position, _groundCheckSize);
    }

   
    public void HandleJump()
    {
        // 1. COYOTE TIME LOGIC
        if (_player.IsGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }


        // 2. JUMP BUFFER LOGIC
        if (GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame())
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter-= Time.deltaTime;
        }

        // 3. EXECUTE JUMP
        if (_coyoteTimeCounter >0f && _jumpBufferCounter>0f)
        {

            if (_player.IsRunning)
            {
                _player.Rb.linearVelocity = new Vector2(_runJumpVelocity.x * _player.DirectionX, _runJumpVelocity.y);
            }
            else
            {
                _player.Rb.linearVelocity = new Vector2(_player.Rb.linearVelocityX, _jumpVelocityY);
            }
            _coyoteTimeCounter = 0f;
            _jumpBufferCounter = 0f;

            AudioManager.Instance.Play(_jumpSound, transform.position);
        }

        //4. Jump with variable height

        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame() && _player.Rb.linearVelocityY >0)
        {
            _player.Rb.linearVelocity = new Vector2(_player.Rb.linearVelocityX, _player.Rb.linearVelocityY* 0.5f);
            _coyoteTimeCounter = 0f;
        }
    }

    public void CheckGround()
    {
        LayerMask groundMask = _player.GroundLayerMask;

        Collider2D collider = Physics2D.OverlapBox(_groundCheckTransform.position, _groundCheckSize, 0f, groundMask);

        bool isHit = collider != null && !collider.isTrigger;

       
        if (isHit )
        {
            _player.IsGrounded = true;
        }
        else
        {
            _player.IsGrounded = false;
        }
       

    }



}
