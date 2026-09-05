using System.Collections;
using UnityEngine;

public class PlayerFalling : MonoBehaviour
{
    [SerializeField] private float _stunnedFallingVelocity = 25f;
    [SerializeField] private float _fallingStunedDuration = 0.5f;
    [SerializeField] private AudioClip _fallingSound;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();

    }

    private IEnumerator FallingStunnedCorountine()
    {
        yield return new WaitForSeconds(_fallingStunedDuration);
        _player.IsFall = false;
        _player.PlayerAnimation.SetTriggerEndCrouch();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_player.GroundLayerMask.Contains(collision.gameObject.layer))
        {
            float fallingVelocity = collision.relativeVelocity.y;
            if (fallingVelocity >= _stunnedFallingVelocity)
            {
                // trigger falling animation
                _player.IsFall = true;
                StartCoroutine(FallingStunnedCorountine());
                _player.PlayerAnimation.SetTriggerStartCrouch();
                // trigger falling sound
                AudioManager.Instance.Play(_fallingSound, transform.position);


            }

        }
    }
    public void HandleFall()
    {
        _player.Rb.linearVelocity = Vector2.zero;
    }
}

