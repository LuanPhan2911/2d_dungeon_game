using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class InteractObject : MonoBehaviour
{
    [SerializeField] private KeyButtonUI _keyButtonUI;
    public bool CanInteract;

    public virtual void Start()
    {
        UpdateInteractKeyUI();
        GameInputManager.Instance.OnRebindCompleted += UpdateInteractKeyUI;
    }
    private void UpdateInteractKeyUI()
    {
       string displayString= GameInputManager.Instance.GetDisplayString(
            GameInputManager.Instance.PlayerActions.Interact, GameInputManager.Instance.CurrentScheme);
        _keyButtonUI.SetDisplayString(displayString);
    }
   

    public virtual void Update()
    {
        if (CanInteract)
        {
            _keyButtonUI.Show();
        }
        else
        {
            _keyButtonUI.Hide();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanInteract = false;
        }
    }






}
