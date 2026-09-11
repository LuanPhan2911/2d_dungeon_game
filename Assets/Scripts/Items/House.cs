using UnityEngine;
using UnityEngine.InputSystem;

public class House : MonoBehaviour
{


    private bool _isOpen = false;

    private PlayerInput _playerInput = null;

    [SerializeField] private HouseDoor _houseDoor;


    private void Update()
    {
        if (_isOpen && _playerInput && _playerInput.actions["Interact"].WasPressedThisFrame())
        {
            _houseDoor.SetOpenDoor();

            // TODO: handle next level
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            _isOpen = true;
            _playerInput = player.GetComponent<PlayerInput>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isOpen = false;
        _playerInput = null;
    }


}
