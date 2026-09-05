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
        if (_player.IsTurning|| _player.IsWallJumping) return;
        // 2 case:
        // grounded: 

        bool isSuddenlyTurn = (_player.IsRightMove && !_player.IsFacingRight) ||
            (_player.IsLeftMove && _player.IsFacingRight);

        if (_player.IsRunning && isSuddenlyTurn)
        {

            ToggleFacingRightWithTurnAnimation();
        }
        else 
        {
            DefaultSpriteFlip();
        }

        
    }
    private void DefaultSpriteFlip()
    {
        if (_player.IsRightMove)
        {
            SetFacingRight(true);
        }
        else if (_player.IsLeftMove)
        {
            SetFacingRight(false);
        }
    }

    public void ToggleFacingRightWithTurnAnimation()
    {
        _player.IsTurning = true;
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
