using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{

    [SerializeField] private float _climbingVelocity = 1f;
    public bool CanClimb = false;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();

    }

    public void HandleClimbing()
    {


       _player.VerticalInput = GameInputManager.Instance.GetVerticalInput();
        _player.HorizontalInput = GameInputManager.Instance.GetHorizontalInput();
        if (CanClimb)
        {
           
            if (_player.IsVerticalMoving)
            {
                _player.IsClimbing = true;
                _player.PlayerAnimation.SetClimbing(true);
                _player.PlayerAnimation.StartCurrentAnimation();
                _player.Rb.linearVelocity = new Vector2(0f, _player.VerticalInput * _climbingVelocity);
            }
            else
            {
                // player is not climbing
                if (_player.IsClimbing)
                {
                    _player.Rb.linearVelocity = Vector2.zero;
                    _player.PlayerAnimation.PauseCurrentAnimation();
                }

                if (_player.IsClimbing && _player.IsGrounded && _player.IsHorizontalMoving)
                {
                    Debug.Log("Player want to movement");
                    _player.IsClimbing = false;
                    _player.PlayerAnimation.SetClimbing(false);
                    _player.PlayerAnimation.StartCurrentAnimation();

                }

                // Stop climb
                if (_player.IsClimbing && GameInputManager.Instance.PlayerJumpAction.WasPressedThisFrame())
                {
                    Debug.Log("Player Stop climbing");
                    _player.IsClimbing = false;
                    _player.PlayerAnimation.SetClimbing(false);
                    _player.PlayerAnimation.StartCurrentAnimation();
                    _player.Rb.linearVelocity = Vector2.zero;
                }
            }




        }
        else
        {
            _player.IsClimbing = false;
            _player.PlayerAnimation.SetClimbing(false);
            _player.PlayerAnimation.StartCurrentAnimation();

        }

    }







}
