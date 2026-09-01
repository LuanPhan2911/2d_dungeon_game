using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPassThroughPlatform : MonoBehaviour
{
    [SerializeField] private float _ignoreCollisonDuration = 0.5f;
    private Collider2D _currentPlatformCollider = null;
    private Player _player;
    private void Awake()
    {
      _player= GetComponent<Player>();
    }
    private void Update()
    {

        if (_currentPlatformCollider != null)
        {
            if (GameInputManager.Instance.IsDownPressed())
            {
                _player.PlayerSprite.IgnoreCollision(_currentPlatformCollider, _ignoreCollisonDuration);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_player.PlatformLayer.Contains(collision.gameObject.layer))
        {
            _currentPlatformCollider = collision.collider;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_player.PlatformLayer.Contains(collision.gameObject.layer))
        {
            _currentPlatformCollider = null;
        }
    }



}
