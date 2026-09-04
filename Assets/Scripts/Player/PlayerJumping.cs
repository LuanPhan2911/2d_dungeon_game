using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    public int JumpRemaining;
    [SerializeField] private float _jumpVelocity = 6f;

    [SerializeField] private float _groundJumpDuration = 0.5f;

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

        

        if (GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame() && _player.IsGrounded )
        {
           
            _isJumpPress = true;
            _jumpDuration =0;
            AudioManager.Instance.Play(_jumpSound, transform.position);
        }

        if (GameInputManager.Instance.PlayerJumpAction.IsPressed() && _isJumpPress)
        {
            _jumpDuration += Time.deltaTime;
           

            if (_jumpDuration < _groundJumpDuration)
            {
                _player.VerticalVelocity = _jumpVelocity;
            }
            else
            {
                _isJumpPress = false;
            }

        }
        if (GameInputManager.Instance.PlayerJumpAction.WasReleasedThisFrame())
        {
            _isJumpPress = false;
        }


    }

    public void GroundCheck()
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
