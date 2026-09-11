using UnityEngine;



public class Ladder : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _topLadderCollider2d;

    [SerializeField] private InteractUI _interactUI;

    private PlayerMovement _player;

   
    private void OnTriggerEnter2D(Collider2D playerCollider)
    {
        
    }

    private void OnTriggerExit2D(Collider2D playerCollider)
    {
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_player != null)
        {
            _interactUI.SetYPosition(_player.transform.position.y);
        }
    }




}
