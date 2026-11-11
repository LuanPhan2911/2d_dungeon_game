using Unity.VisualScripting;
using UnityEngine;


public class InteractObject : MonoBehaviour
{
    [SerializeField] private InteractKeyUI _interactKeyUI;
    public bool CanInteract;


    public virtual void Update()
    {
        if (CanInteract)
        {
            _interactKeyUI.Show();
        }
        else
        {
            _interactKeyUI.Hide();
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
