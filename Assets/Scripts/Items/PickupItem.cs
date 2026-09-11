using UnityEngine;

public class PickupItem : MonoBehaviour
{


    [SerializeField] private ItemData _itemData;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {

            InventoryManager.Instance.AddItem(_itemData);
            Destroy(gameObject);
        }
    }
}
