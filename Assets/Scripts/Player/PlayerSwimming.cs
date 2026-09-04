using System.Collections;
using UnityEngine;

public class PlayerSwimming : MonoBehaviour
{

 
    //[SerializeField] private AudioClip _swimmingSound;
    //[SerializeField] private PlayerVelocity _waterVelocity;
    //private Player _player;

    //private Coroutine _soundCoroutine;


    //private void Awake()
    //{

    //    _player = GetComponent<Player>();

    //}
    //private void Update()
    //{
    //    if (_player.IsSwimming && _soundCoroutine== null)
    //    {
    //        _soundCoroutine = StartCoroutine(PlayerSwimmingSoundCorountine());
    //    }
    //}
    //private IEnumerator PlayerSwimmingSoundCorountine()
    //{
    //    AudioManager.Instance.Play(_swimmingSound, transform.position);
    //    yield return new WaitForSeconds(_swimmingSound.length);
    //    _soundCoroutine = null;

    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (_player.WaterLayerMask .Contains(collision.gameObject.layer))
    //    {
    //        _player.IsSwimming = true;
    //        _player.PlayerAnimation.SetSwimming(true);
    //        _player.CurrentPlayerVelocity = _waterVelocity;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (_player.WaterLayerMask.Contains(collision.gameObject.layer))
    //    {
    //        _player.IsSwimming = false;
    //        _player.CurrentPlayerVelocity = _player.DefaultPlayerVelocity;
    //        _player.PlayerAnimation.SetSwimming(false);
            
    //    }
    //}
}
