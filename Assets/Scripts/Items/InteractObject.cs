using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractObject : MonoBehaviour
{
    [SerializeField] private ItemData _needItemData;

    [SerializeField] private InteractUI _interactUI;

    public bool CanInteract;

  
    public event EventHandler InteractSuccessAction;
    public event EventHandler InteractFailAction;
    

    private void Update()
    {
        if (CanInteract )
        {
            if(GameInputManager.Instance.PlayerMoveAction.WasPressedThisFrame() &&
                GameInputManager.Instance.IsUpPressed())
            {
                if (InventoryManager.Instance.HasItem(_needItemData))
                {
                    InteractSuccessAction?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Debug.Log("Need required item");
                    InteractFailAction?.Invoke(this, EventArgs.Empty);
                }
            }
           
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            CanInteract = true;
            _interactUI.Show();
            _interactUI.SetInteractText(GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.UpBinding));

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            CanInteract = false;
            _interactUI.Hide();
            
        }
    }

   

}
