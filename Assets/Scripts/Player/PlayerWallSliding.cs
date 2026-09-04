using System.Collections;
using UnityEngine;

public class PlayerWallSliding : MonoBehaviour
{
    
    [SerializeField] private Vector2 _wallCheckSize;

    [SerializeField] private Transform _wallCheckLeft;
    [SerializeField] private Transform _wallCheckRight;

    [SerializeField] private float _sliddingSpeed = 2f;

    private const float _threshold = 0.1f;





    private Player _player;
    private PlayerWallJumping _playerWallJumping;

    private Coroutine _delayWallSlidingCoroutine;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerWallJumping = GetComponent<PlayerWallJumping>();
    }


    private IEnumerator DelayWallSliding()
    {
        yield return new WaitForSeconds(0.1f);

        _player.IsWallSliding = false;
        _delayWallSlidingCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(_wallCheckLeft.position, _wallCheckSize);
        Gizmos.DrawWireCube(_wallCheckRight.position, _wallCheckSize);

    }

    public void WallCheck()
    {
       
       

        if (_player.IsGrounded) return;

        Collider2D leftCollider = Physics2D.OverlapBox(_wallCheckLeft.position, _wallCheckSize, 0f, _player.WallLayerMask);
        Collider2D rightCollider = Physics2D.OverlapBox(_wallCheckRight.position, _wallCheckSize, 0f, _player.WallLayerMask);
       
        if (_player.HorizontalVelocity != 0 && (leftCollider!= null || rightCollider!=null) )
        {   
            _player.IsWallSliding = true;
            _player.VerticalVelocity = Mathf.Max(_player.Rb.linearVelocityY, -_sliddingSpeed);

           
            // set wall jump direction
            if (leftCollider != null)
            {
                _player.PlayerSprite.Flip(false);
                _playerWallJumping.WallJumpDirection = 1;
            }else if(rightCollider!= null)
            {
                _player.PlayerSprite.Flip(true);
                _playerWallJumping.WallJumpDirection = -1;
            }
        }
        else
        {
            if (_player.IsWallSliding && _delayWallSlidingCoroutine == null)
            {
                _delayWallSlidingCoroutine = StartCoroutine(DelayWallSliding());
            }
        }
       

       
    }
}
