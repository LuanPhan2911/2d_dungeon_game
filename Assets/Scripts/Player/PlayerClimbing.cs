using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{


    public bool CanClimb = false;




    [SerializeField] private float _distanceCheck = 0.5f;

    [SerializeField] private Transform _ledgeCheckPoint;

    [SerializeField] private Vector2 _climbOffset = new Vector2(0.5f, 1f);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 from = _ledgeCheckPoint.position;
        int direction = _player != null ? _player.LastHorizontalInput : 1;
        Vector3 to = from + Vector3.right * direction * _distanceCheck ;

        Gizmos.DrawLine( from, to );


    }

    private Player _player;
    private PlayerWallJumping _playerWallJumping;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerWallJumping = GetComponent<PlayerWallJumping>();


    }

    public void HandleClimbing()
    {
        if (_player.IsStopAction) return;


        if (CanClimb)
        {
           
            _player.IsClimbing = true;
            _player.Rb.linearVelocity = Vector2.zero;

            _player.Rb.bodyType = RigidbodyType2D.Kinematic;

            _player.PlayerAnimation.SetTriggerClimbUp();
        }
    }
    public void FinishClimb()
    {
        _player.IsClimbing = false;
        _player.Rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 climbPosition= new Vector2(transform.position.x + _player.LastHorizontalInput * _climbOffset.x, transform.position.y + _climbOffset.y);


        _player.SetPosition(climbPosition);

    }

    public void CheckLedge()
    {
        CanClimb = false;
        if (_player.IsGrounded || _player.IsClimbing) return;

        Vector2 origin = (Vector2)_ledgeCheckPoint.position;
        RaycastHit2D isLedgeHit = Physics2D.Raycast(origin, Vector2.right * _player.LastHorizontalInput, _distanceCheck, _player.WallLayerMask);

        CanClimb = _playerWallJumping.CanWallSlide && !isLedgeHit  ;
    }







}
