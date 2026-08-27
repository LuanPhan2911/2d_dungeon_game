using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    public int JumpRemaining;
    [SerializeField] private float _jumpVelocity = 6f;

    [SerializeField] private float _maxGroundJumpDuration = 0.5f;

    [SerializeField] private AudioClip _jumpSound;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(1, 0.2f);

   

    private Player _player;
   

    private float _jumpDuration;
    private bool _isJumpPress;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_groundCheckTransform.position, _groundCheckSize);
    }

    public void HandleJumpVelocity()
    {
        _player.VerticalVelocity = _player.Rb.linearVelocityY;
        _player.Rb.gravityScale = _player.GravityScale;
    }
    public void HandleJump()
    {
        

        if (_player.PlayerInput.actions["Jump"].WasPressedThisFrame() && JumpRemaining > 0)
        {
            JumpRemaining--;
            _isJumpPress = true;
            _jumpDuration =0;
            AudioManager.Instance.Play(_jumpSound, transform.position);
        }

        if (_player.PlayerInput.actions["Jump"].IsPressed() && _isJumpPress)
        {
            _jumpDuration += Time.deltaTime;
            bool isFirstJump = _player.JumpAvailable - JumpRemaining == 1;
            bool isAirJump = JumpRemaining == 0;
            float _maxDuration = isFirstJump ? _maxGroundJumpDuration : _player.MaxAirJumpDuration;

            if (_jumpDuration < _maxDuration)
            {
              
                if (isFirstJump)
                {
                    _player.VerticalVelocity = _jumpVelocity;
                }
                else if (isAirJump)
                {
                    _player.VerticalVelocity = _player.AirJumpVelocity;
                    _player.PlayAirJumpFX();
                }
            }
            else
            {
                _isJumpPress = false;
            }

        }
        if (_player.PlayerInput.actions["Jump"].WasReleasedThisFrame())
        {
            _isJumpPress = false;
        }


    }

    public void GroundCheck()
    {
        _player.IsGrounded = false;
      
      
        LayerMask groundMask = _player.GroundLayerMask;

        Collider2D collider = Physics2D.OverlapBox(_groundCheckTransform.position, _groundCheckSize, 0f, groundMask);

        bool isHit = collider != null && !collider.isTrigger;

        Debug.Log(collider);
        if (( isHit|| _player.IsSwimming) && !_isJumpPress  )
        {
            _player.IsGrounded = true;
            JumpRemaining = _player.JumpAvailable;
        }

    }



}
