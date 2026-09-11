using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField] private AudioClip _coinSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            // Add coin to player's score
         
            // Destroy the coin object
            gameObject.SetActive(false);

            AudioManager.Instance.Play(_coinSound, transform.position);


        }

    }
}
