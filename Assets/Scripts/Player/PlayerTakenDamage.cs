using System.Collections;
using UnityEngine;

public class PlayerTakenDamage : MonoBehaviour
{

    [SerializeField] private AudioClip _hurtSound;


    private Player _player;
    private DamageFlash _damageFlash;

   

    private void Awake()
    {
        _player = GetComponent<Player>();
        _damageFlash = GetComponent<DamageFlash>();
    }
    public void TakeDamage(int damage)
    {
        //if (_player.IsInvincible) return;


      //_player.Health= _player.Health - damage;
      
      //  if (_player.Health <= 0)
      //  {
      //      // Handle player death (e.g., reload the scene, show game over screen, etc.)

      //      //SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
      //      Debug.Log("Died!!!");

      //      return;

      //  }
       

        //StartCoroutine(TakeDamageCorountine());


    }

    //private IEnumerator TakeDamageCorountine()
    //{
        //_player.IsInvincible = true;
        //AudioManager.Instance.Play(_hurtSound, transform.position);
        //_damageFlash.PingPongFlash(_player.InvincibilityDuration);

        //yield return new WaitForSeconds(_player.InvincibilityDuration);

        //_player.IsInvincible = false;
    //}
}
