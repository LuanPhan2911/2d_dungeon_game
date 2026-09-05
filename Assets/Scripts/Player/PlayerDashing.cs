using System.Collections;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{



    public bool CanDash = true;

    [SerializeField] private float _dashCooldown = 0.75f;

    [SerializeField] private float _dashDuration = 0.2f;

    [SerializeField] private float _dashVelocityX = 20f;

    [SerializeField] private PlayerDashFX _dashFX;

    private Player _player;

    private float _dashCooldownTimer = 0f;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }



    public void HandleDash()
    {
        if(!CanDash && !_player.IsDashing)
        {
            _dashCooldownTimer -= Time.deltaTime;

            if (CanResetDash())
            {
                _dashCooldownTimer = 0f;
                CanDash = true;
            }
        }

        if(GameInputManager.Instance.PlayerRunAction.WasPressedThisFrame() && CanDash)
        {

            StartCoroutine(Dash());
        }

    }

    private IEnumerator Dash()
    {
        CanDash = false;

        _player.IsDashing = true;

        float originalGravityScale = _player.Rb.gravityScale;
        _player.Rb.gravityScale = 0f;

        _player.Rb.linearVelocity = new Vector2(_player.DirectionX * _dashVelocityX, 0f);

        _dashFX.PlayDashFX(true);

        yield return new WaitForSeconds(_dashDuration);

        _player.Rb.gravityScale= originalGravityScale;


        _player.IsDashing = false;
        _dashCooldownTimer = _dashCooldown;
        _dashFX.PlayDashFX(false);




    }
    


    private bool CanResetDash()
    {

        return (_player.IsGrounded || _player.IsWallSliding) && _dashCooldownTimer <= 0f;
    }
}
