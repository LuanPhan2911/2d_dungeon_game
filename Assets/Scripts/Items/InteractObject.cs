using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractObject : MonoBehaviour
{
    [SerializeField] private ItemSO _needItemData;

    [SerializeField] private InteractUI _interactUI;

    public bool CanInteract;

  
    public event EventHandler InteractSuccessAction;
    public event EventHandler InteractFailAction;
    

    private void Update()
    {
        if (CanInteract )
        {
            if(GameInputManager.Instance.PlayerActions.Move.WasPressedThisFrame() &&
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


   

   

}
