using UnityEngine;



public class Ladder : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _topLadderCollider2d;

    [SerializeField] private InteractUI _interactUI;

    private Player _player;

    private void Update()
    {

        if (_player != null)
        {
            if (_player.IsClimbing)
            {
                _player.PlayerSprite.IgnoreCollision(_topLadderCollider2d, true);
            }
            else
            {
                _player.PlayerSprite.IgnoreCollision(_topLadderCollider2d, false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D playerCollider)
    {
        if (playerCollider.TryGetComponent(out PlayerClimbing playerClimbing))
        {
            playerClimbing.CanClimb = true;
            _player = playerClimbing.GetComponent<Player>();
            _interactUI.Show();

            


        }
    }

    private void OnTriggerExit2D(Collider2D playerCollider)
    {
        if (playerCollider.TryGetComponent(out PlayerClimbing playerClimbing))
        {
            playerClimbing.CanClimb = false;
            _player = null;
            _interactUI.Hide(); 


        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_player != null)
        {
            _interactUI.SetYPosition(_player.transform.position.y);
        }
    }




}
