
using UnityEngine;


public class PlayerAnimation : MonoBehaviour
{


    private Animator _animator;

    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";
    const string IS_CLIMBING = "IsClimbing";
    const string IS_SWIMMING = "IsSwimming";
    const string IS_SLIDING = "IsSliding";

    const string IS_GROUNDED = "IsGrounded";
    const string IS_DASHING = "IsDashing";

    const string IS_CROUCH_WALKING = "IsCrouchWalking";

    // trigger parameter 

 

    const string START_CROUCH= "StartCrouch";

    const string END_CROUCH = "EndCrouch";

    const string TURN = "Turn";

    private Player _player;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    public void UpdateAnimation()
    {
        float horizontal = GetHorizontalValue();
        float vertical = GetVerticalValue();


        SetHorizontal(horizontal);
        SetVertical(vertical);

        SetGrounded(_player.IsGrounded);
        SetSliding(_player.IsWallSliding);
        SetCrouchWalking(_player.IsCrouchWalking);
        SetDashing(_player.IsDashing);
    }

    private float GetHorizontalValue()
    {
        float horizontal = 0;
        if (_player.IsRunning && _player.IsHorizontalMoving)
        {
            horizontal = 2f;
        }
        else
        {
            horizontal = Mathf.Abs(_player.HorizontalInput);
        }
        return horizontal;
    }


    public void SetHorizontal(float horizontal)
    {
        _animator.SetFloat(HORIZONTAL, Mathf.Abs(horizontal));
    }
    public void SetClimbing(bool isClimbing)
    {
        _animator.SetBool(IS_CLIMBING, isClimbing);
    }
    public void SetVertical(float vertical)
    {
        _animator.SetFloat (VERTICAL, vertical);
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(IS_GROUNDED, isGrounded);
    }

    public void SetTriggerStartCrouch()
    {
        _animator.SetTrigger(START_CROUCH);
    }
    public void SetTriggerEndCrouch()
    {
        _animator.SetTrigger(END_CROUCH);
    }
   
    public void SetSwimming(bool isSwimming)
    {
        _animator.SetBool(IS_SWIMMING, isSwimming);
    }

    public void PauseCurrentAnimation()
    {
        _animator.speed = 0f;
    }
    public void StartCurrentAnimation()
    {
        _animator.speed = 1f;
    }
    public void SetTriggerTurnAround()
    {
        _animator.SetTrigger(TURN);
    }

    public void SetDashing(bool isDashing)
    {
        _animator.SetBool(IS_DASHING, isDashing);
    }
    
    private float GetVerticalValue()
    {
        float vertical = 0;

        if (_player.Rb.linearVelocityY > 0)
        {
            vertical = 1;
        }
        else if (_player.Rb.linearVelocityY < 0)
        {
            vertical = -1;
        }
        return vertical;
    }
   
    public void SetSliding(bool isSliding)
    {
        _animator.SetBool(IS_SLIDING, isSliding);
    }

    public void SetCrouchWalking(bool isCrouchWalking)
    {
        _animator.SetBool(IS_CROUCH_WALKING, isCrouchWalking);
    }
}
