
using UnityEngine;


public class PlayerAnimation : MonoBehaviour
{


   [SerializeField] private Animator _animator;

    
    


    const string HORIZONTAL = "Horizontal";
    const string IS_FALLING = "IsFalling";

    const string IS_WALL_SLIDING = "IsWallSliding";
    const string IS_JUMPING = "IsJumping";
    const string IS_DASHING = "IsDashing";
    const string IS_CROUCHING = "IsCrouching";

    const string IS_CROUCH_WALKING = "IsCrouchWalking";

    const string ATTACK_COMBO = "AttackCombo";

 

    // trigger parameter 

    const string ATTACK = "Attack";

    const string TURN = "Turn";

    const string CLIMB_UP = "ClimbUp";

    private Player _player;


    private void Awake()
    {
       
        _player = GetComponent<Player>();
    }

    public void UpdateAnimation()
    {
        float horizontal = Mathf.Abs(_player.HorizontalInput);
       
        SetHorizontal(horizontal);
        SetFalling(_player.IsFalling);
        SetJumping(_player.IsJumping || _player.IsWallJumping);
        SetWallSliding(_player.IsWallSliding);
        SetCrouchWalking(_player.IsCrouchWalking);
        SetDashing(_player.IsDashing);
        SetCrouching(_player.IsCrouching);
    }

    

    public void SetTriggerAttack()
    {
        _animator.SetTrigger(ATTACK);
    }
    public void SetAttackCombo(int comboStep)
    {
        _animator.SetFloat(ATTACK_COMBO, comboStep);
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

    public void SetCrouching(bool isCrouching)
    {
        _animator.SetBool(IS_CROUCHING, isCrouching);
    }
   
 

    public void PauseCurrentAnimation()
    {
        _animator.speed = 0f;
    }
    public void StartCurrentAnimation()
    {
        _animator.speed = 1f;
    }
    public void SetTriggerTurn()
    {
        _animator.SetTrigger(TURN);
    }

    public void SetDashing(bool isDashing)
    {
        _animator.SetBool(IS_DASHING, isDashing);
    }
    public void SetTriggerClimbUp()
    {
        _animator.SetTrigger(CLIMB_UP);
    }
    public void SetWallSliding(bool isSliding)
    {
        _animator.SetBool(IS_WALL_SLIDING, isSliding);
    }

    public void SetCrouchWalking(bool isCrouchWalking)
    {
        _animator.SetBool(IS_CROUCH_WALKING, isCrouchWalking);
    }
}
