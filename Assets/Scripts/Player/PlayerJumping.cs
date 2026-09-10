using System.Diagnostics;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
   
    [SerializeField] private AudioClip _jumpSound;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.3f, 0.03f);





    private Player _player;



    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }

   
    public void HandleJump()
    {

        if (_player.IsJumping && _player. Rb.linearVelocityY < 0)
        {
           _player. IsFalling = true;
           _player. IsJumping = false;
        }


        if (CanJump() && _player.LastPressJumpTime > 0)
        {
            _player.IsJumping = true;
            _player.IsWallJumping = false;
            _player.IsJumpCut = false;
            _player.IsFalling = false;
           
            ExecuteJump();
          
            AudioManager.Instance.Play(_jumpSound, transform.position);
        }

       
    }

    public void CheckGround()
    {   if (_player.IsDashing || _player.IsJumping) return;
         
         if( Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, _player.GroundLayerMask) )
        {
          _player.  LastOnGroundTime = _player.Data.coyoteTime;
        }
    }


    private void ExecuteJump()
    {
        _player.LastPressJumpTime = 0;
        _player.LastOnGroundTime = 0;

        #region Perform Jump

        float force = _player.Data.jumpForce;

        if(_player.Rb.linearVelocityY < 0)
        {
            force -= _player.Rb.linearVelocityY;
        }
        _player.Rb.AddForce(force * Vector2.up, ForceMode2D.Impulse);
        #endregion
    }

    private bool CanJump()
    {
        return _player.LastOnGroundTime > 0 && !_player.IsJumping ;
    }
    public bool CanJumpCut()
    {
        return _player.IsJumping && _player.Rb.linearVelocityY > 0;
    }



}
