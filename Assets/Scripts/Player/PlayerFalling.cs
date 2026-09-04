using System.Collections;
using UnityEngine;

public class PlayerFalling : MonoBehaviour
{


    [SerializeField] private float _maxFallingVelocity = 12f;
    [SerializeField] private float _fallingStunedDuration = 0.5f;
    [SerializeField] private AudioClip _fallingSound;
    [SerializeField] private float _fallingSpeedMultiplier = 2f;
    private Player _player;


    private void Awake()
    {
        _player = GetComponent<Player>();

    }



    public void HandleFallingVelocity()
    {
        _player.Rb.gravityScale = _player.GravityScale * _fallingSpeedMultiplier;
        _player.VerticalVelocity = Mathf.Max(_player.Rb.linearVelocityY, -_maxFallingVelocity);
    }
    private IEnumerator FallingStunnedCorountine()
    {
        yield return new WaitForSeconds(_fallingStunedDuration);
        _player.IsFall = false;
        _player.PlayerAnimation.SetTriggerEndCroch();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_player.GroundLayerMask.Contains(collision.gameObject.layer))
        {
            float fallingVelocity = collision.relativeVelocity.y;
            HanleFallGreatHeight(fallingVelocity);

        }
    }
    private void HanleFallGreatHeight(float fallVelocity)
    {
        if (fallVelocity >= _maxFallingVelocity)
        {
            // trigger falling animation
            _player.IsFall = true;
            StartCoroutine(FallingStunnedCorountine());
            _player.PlayerAnimation.SetTriggerStartCroch();
            // trigger falling sound
            AudioManager.Instance.Play(_fallingSound, transform.position);


        }
    }
}

