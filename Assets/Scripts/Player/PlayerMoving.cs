using System.Collections;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private AudioClip[] _walkingSounds;
   
    private Player _player;
    private float _footStepSoundRate = 0.3f;
    private Coroutine _footStepCoroutine = null;

    private void Awake()
    {
        _player = GetComponent<Player>();

    }
    public void HandleMoving()
    {
        float horizontalInput = GameInputManager.Instance.GetHorizontalInput();

        float horizontalVelocity = _player.CurrentPlayerVelocity.HorizontalVelocity;
        _player.HorizontalVelocity = horizontalInput * horizontalVelocity;

        if (_player.IsGrounded && Mathf.Abs(horizontalInput) > 0.1f)
        {
            if(_footStepCoroutine== null)
            {
                _footStepCoroutine = StartCoroutine(PlayFootStepCoroutine());
            }
        }
     
    }
    private IEnumerator PlayFootStepCoroutine()
    {
        AudioManager.Instance.Play(_walkingSounds, transform.position);
        yield return new WaitForSeconds(_footStepSoundRate);
        _footStepCoroutine = null;

    }
}
