
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerAnimation : MonoBehaviour
{


    private Animator animator;

    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";
    const string IS_CLIMBING = "IsClimbing";
    const string IS_SWIMMING = "IsSwimming";
    const string IS_SLIDING = "IsSliding";

    const string IS_GROUNDED = "IsGrounded";

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


    

    public void SetHorizontal(float horizontal)
    {
        animator.SetFloat(HORIZONTAL, Mathf.Abs(horizontal));
    }
    public void SetClimbing(bool isClimbing)
    {
        animator.SetBool(IS_CLIMBING, isClimbing);
    }
    public void SetVertical(float vertical)
    {
        animator.SetFloat (VERTICAL, vertical);
    }

    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool(IS_GROUNDED, isGrounded);
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
        float horizontal = Mathf.Clamp01(Mathf.Abs(_player.HorizontalVelocity));
        float vertical = 0;

        if(_player.VerticalVelocity> 0)
        {
            vertical = 1;
        }
        else if(_player.VerticalVelocity < 0) 
        {
            vertical = -1;
        }

        SetHorizontal(horizontal);
        SetVertical(vertical);

        SetGrounded(_player.IsGrounded);
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
