using UnityEngine;

public class CheckPoint : InteractObject
{



    public override  void Update()
    {

        base.Update();

        if (CanInteract)
        {
            if (GameInputManager.Instance.PlayerActions.Interact.WasPressedThisFrame())
            {
                // show checkpoint panel

                CheckPointPanel.Instance.Show();
            }
        }
    }
}
