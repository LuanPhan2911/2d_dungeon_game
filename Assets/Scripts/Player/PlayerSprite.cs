using System.Collections;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CapsuleCollider2D _playerCollider;
    private Player _player;
    public float DefaultColliderSizeX { get; private set; }
    public float DefaultColliderSizeY { get; private set; }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerCollider = GetComponent<CapsuleCollider2D>();


        DefaultColliderSizeX = _playerCollider.size.x;
        DefaultColliderSizeY = _playerCollider.size.y;
    }

    public void UpdateSprite()

    {
        // 2 case:
        // grounded: 

        bool isSuddenlyTurn = (_player.HorizontalInput >0 && !_player.IsFacingRight) ||
            (_player.HorizontalInput< 0 && _player.IsFacingRight);

        if (_player.IsRunning)
        {

            if (isSuddenlyTurn)
            {
                ToggleFacingRightWithTurnAnimation();
            }
        }
        else
        {
            DefaultSpriteFlip();
        }

        
    }
    private void DefaultSpriteFlip()
    {
        if (_player.HorizontalInput > 0)
        {
            SetFacingRight(true);
        }
        else if (_player.HorizontalInput < 0)
        {
            SetFacingRight(false);
        }
    }

    public void ToggleFacingRightWithTurnAnimation()
    {
        _player.PlayerAnimation.SetTriggerTurnAround();
        _player.IsFacingRight = !_player.IsFacingRight;
        
    }
    public void SetFacingRight(bool isFacingRight)
    {
        _player.IsFacingRight = isFacingRight;
        _player.SpriteRenderer.flipX = !isFacingRight;

    }

    public void IgnoreCollision(Collider2D otherCollider, float duration)
    {
        StartCoroutine(IgnoreCollisionCoroutine(otherCollider, duration));
    }
    public void IgnoreCollision(Collider2D otherCollider, bool isIgnore)
    {
        Physics2D.IgnoreCollision(_playerCollider, otherCollider, isIgnore);
    }

    private IEnumerator IgnoreCollisionCoroutine(Collider2D otherCollider, float duration)
    {
        Physics2D.IgnoreCollision(_playerCollider, otherCollider, true);

        yield return new WaitForSeconds(duration);

        Physics2D.IgnoreCollision(_playerCollider, otherCollider, false);

    }

}
