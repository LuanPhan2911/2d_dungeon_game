
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerAnimation : MonoBehaviour
{


    private Animator animator;

    const string HORIZONTAL = "Horizontal";
    const string IS_JUMPING = "IsJumping";
    const string IS_CLIMBING = "IsClimbing";
    const string IS_SWIMMING = "IsSwimming";
    const string IS_SLIDING = "IsSliding";

    const string IS_CROCHWALKING = "IsCrochWalking";

    // trigger parameter for crouching

 

    const string START_CROCH= "StartCroch";

    const string END_CROCH = "EndCroch";

    private Player _player;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }


    public void SetJumping(bool isJumping)
    {
        animator.SetBool(IS_JUMPING, isJumping);
    }

    public void SetHorizontal(float horizontal)
    {
        animator.SetFloat(HORIZONTAL, Mathf.Abs(horizontal));
    }
    public void SetClimbing(bool isClimbing)
    {
        animator.SetBool(IS_CLIMBING, isClimbing);
    }

    public void SetTriggerStartCroch()
    {
        animator.SetTrigger(START_CROCH);
    }
    public void SetTriggerEndCroch()
    {
        animator.SetTrigger(END_CROCH);
    }
   
    public void SetSwimming(bool isSwimming)
    {
        animator.SetBool(IS_SWIMMING, isSwimming);
    }

    public void PauseCurrentAnimation()
    {
        animator.speed = 0f;
    }
    public void StartCurrentAnimation()
    {
        animator.speed = 1f;
    }
    
    public void UpdateAnimation()
    {
        SetHorizontal(_player.HorizontalVelocity);
        SetJumping(!_player.IsGrounded);
        SetSliding(_player.IsWallSliding);
        SetCrochWalking(_player.IsCrochWalking);
    }
    public void SetSliding(bool isSliding)
    {
        animator.SetBool(IS_SLIDING, isSliding);
    }

    public void SetCrochWalking(bool isCrochWalking)
    {
        animator.SetBool(IS_CROCHWALKING, isCrochWalking);
    }
}
