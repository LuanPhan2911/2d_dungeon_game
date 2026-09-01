using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{

    [SerializeField] private float _climbingSpeed = 1f;
    public bool CanClimb = false;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();

    }

    public void HandleClimbing()
    {


        float verticalInput = GameInputManager.Instance.GetVerticalInput();
        float horizontalInput = GameInputManager.Instance.GetHorizontalInput();
        if (CanClimb)
        {
            const float thresholdInput = 0.1f;
            if (Mathf.Abs(verticalInput) > thresholdInput)
            {
                _player.IsClimbing = true;
                _player.PlayerAnimation.SetClimbing(true);
                _player.PlayerAnimation.StartCurrentAnimation();
                _player.VerticalVelocity = verticalInput * _climbingSpeed;
            }
            else
            {
                // player is not climbing
                if (_player.IsClimbing)
                {
                    _player.VerticalVelocity = 0f;
                    _player.PlayerAnimation.PauseCurrentAnimation();
                }

                if (_player.IsClimbing && _player.IsGrounded && Mathf.Abs(horizontalInput) > thresholdInput)
                {
                    Debug.Log("Player want to movement");
                    _player.IsClimbing = false;
                    _player.PlayerAnimation.SetClimbing(false);
                    _player.PlayerAnimation.StartCurrentAnimation();

                }

                // Stop climb
                if (_player.IsClimbing && GameInputManager.Instance.PlayerJumpAction.IsPressed())
                {
                    Debug.Log("Player Stop climbing");
                    _player.IsClimbing = false;
                    _player.PlayerAnimation.SetClimbing(false);
                    _player.PlayerAnimation.StartCurrentAnimation();
                    _player.VerticalVelocity = 0f;
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
