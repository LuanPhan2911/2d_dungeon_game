using UnityEngine;

public class CheckPoint : InteractObject
{



    public override void Start()
    {
        base.Start();
        GameInputManager.Instance.PlayerActions.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (CanInteract)
        {
            CheckPointPanel.Instance.Show();
        }
        else
        {
            CheckPointPanel.Instance.Hide();
        }
    }
}
