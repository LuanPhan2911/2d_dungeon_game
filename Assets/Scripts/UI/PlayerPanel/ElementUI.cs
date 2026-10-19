using UnityEngine;


public class ElementUI : MonoBehaviour
{


    [SerializeField] private ElementIconUI _elementIconUIPrefab;

    public PlayerElement PElement => PlayerElement.Instance;

    private void Start()
    {
        PElement.OnPlayerElementSlotChanged += PElement_OnPlayerElementSlotChanged;
    }

    private void PElement_OnPlayerElementSlotChanged()
    {
        transform.DestroyChildren();
        for (int i = 0; i < PElement.PlayerElementSlotArray.Length; i++)
        {
            PlayerElementSlot playerElementSlot = PElement.PlayerElementSlotArray[i];
            ElementIconUI elementIconUI = Instantiate(_elementIconUIPrefab, transform);

            Sprite elementSprite = playerElementSlot.PlayerElementType.DataSO.element.sprite;
            elementIconUI.SetElementSprite(elementSprite);
        }
    }

   
}
