
using UnityEngine;


public class PlayerAnimation : MonoBehaviour
{


  

    const string HORIZONTAL = "Horizontal";
    const string IS_FALLING = "IsFalling";

    const string IS_WALL_SLIDING = "IsWallSliding";
    const string IS_JUMPING = "IsJumping";
    const string IS_DASHING = "IsDashing";

    // trigger parameter 

    const string HORIZONTAL_ATTACK = "HorizontalAttack";
    const string UPWARD_ATTACK = "UpwardAttack";
    const string DOWNWARD_ATTACK = "DownwardAttack";
    private PlayerMovement _playerMovement;
    private Animator _animator;


    private void Awake()
    {
       
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
    }

    public void UpdateAnimation()
    {
        float horizontal = Mathf.Abs(_playerMovement.HorizontalInput);
       
        SetHorizontal(horizontal);
        SetFalling(_playerMovement.IsFalling);
        SetJumping(_playerMovement.IsJumping || _playerMovement.IsWallJumping);
        SetWallSliding(_playerMovement.IsWallSliding);
        SetDashing(_playerMovement.IsDashing);
       
    }

    

    public void SetTriggeHorizontalAttack()
    {
        _animator.SetTrigger(HORIZONTAL_ATTACK);
    }
    public void SetTriggerUpwardAttack()
    {
        _animator.SetTrigger(UPWARD_ATTACK);
    }
    public void SetTriggerDownwardAttack()
    {
        _animator.SetTrigger(DOWNWARD_ATTACK);
    }
   
    public void SetHorizontal(float horizontal)
    {
        _animator.SetFloat(HORIZONTAL, Mathf.Abs(horizontal));
    }
  
    public void SetFalling(bool isFalling)
    {
        _animator.SetBool(IS_FALLING, isFalling);
    }
   

    public void SetJumping(bool isJumping)
    {
        _animator.SetBool(IS_JUMPING, isJumping);
    }

    public void SetDashing(bool isDashing)
    {
        _animator.SetBool(IS_DASHING, isDashing);
    }
   
    public void SetWallSliding(bool isSliding)
    {
        _animator.SetBool(IS_WALL_SLIDING, isSliding);
    }

  
}
