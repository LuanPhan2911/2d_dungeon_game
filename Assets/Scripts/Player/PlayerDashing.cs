using System.Collections;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{



    public bool CanDash = true;

    public bool CanResetDash ;

    [SerializeField] private float _dashCooldown = 0.75f;

    [SerializeField] private float _dashDuration = 0.2f;

    [SerializeField] private float _dashVelocityX = 20f;

    [SerializeField] private PlayerDashFX _dashFX;

    private Player _player;

    private float _dashCooldownCounter = 0f;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }



    public void HandleDashing()
    {
        if (_player.IsCrouching || _player.IsStopAction)
        {
            CanDash = false;
            return;
        }

        if(!CanDash && !_player.IsDashing)
        {
            _dashCooldownCounter -= Time.deltaTime;
            ResetDash();


        }

        if(GameInputManager.Instance.PlayerRunAction.WasPressedThisFrame() && CanDash)
        {

            StartCoroutine(Dash());
        }

        // after dash if continue press dash button, player sprint

        HandleRun();
    }

    private void HandleRun()
    {
        if (!_player.IsGrounded)
        {
            _player.IsRunning = false;
            return;
        }
        if (GameInputManager.Instance.PlayerRunAction.IsPressed() && _player.IsHorizontalMoving)
        {
            _player.IsRunning = true;

            TriggerTurn();
        }
        else
        {
            _player.IsRunning = false;
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        CanResetDash = false;

        _player.IsDashing = true;

        float originalGravityScale = _player.Rb.gravityScale;
        _player.Rb.gravityScale = 0f;

        int direction = _player.IsWallSliding ? _player.FacingDirection : _player.LastHorizontalInput;

        bool isFacingRight = direction == 1;

        _player.PlayerSprite.SetFacingRight(isFacingRight);

        _player.Rb.linearVelocity = new Vector2(direction * _dashVelocityX, 0f);

        _dashFX.PlayDashFX(true);

        yield return new WaitForSeconds(_dashDuration);

        _player.Rb.gravityScale= originalGravityScale;


        _player.IsDashing = false;
        _dashCooldownCounter = _dashCooldown;
        _dashFX.PlayDashFX(false);




    }
    

    private void ResetDash()
    {
        if (_player.IsGrounded || _player.IsWallSliding)
        {
            CanResetDash = true;
        }
       
        if (CanResetDash && _dashCooldownCounter <= 0f)
        {
            _dashCooldownCounter = 0f;
            CanDash = true;
        }
    }
   

    private void TriggerTurn()
    {
        if ((_player.IsRightMove && !_player.IsFacingRight) ||(_player.IsLeftMove && _player.IsFacingRight))
        {
            _player.IsTurning = true;
            _player.PlayerAnimation.SetTriggerTurn();
        }
    }


    public void FinishTurn()
    {
        _player.IsTurning = false;
        
    }
}
