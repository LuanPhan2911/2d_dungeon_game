using UnityEngine;


public class ElementUI : MonoBehaviour
{
    [SerializeField] private ElementIconUI[] _elementIconUIArray;

    public PlayerElement PElement => PlayerElement.Instance;

    private void Start()
    {
        PElement_OnPlayerElementSlotChanged();
        PElement.OnPlayerElementSlotChanged += PElement_OnPlayerElementSlotChanged;
    }

    private void PElement_OnPlayerElementSlotChanged()
    {
       
        for (int i = 0; i < PElement.PlayerElementSlotArray.Length; i++)
        {
            PlayerElementSlot playerElementSlot = PElement.PlayerElementSlotArray[i];
            Sprite elementSprite = playerElementSlot.PlayerElementType.DataSO.elementSO.sprite;
            _elementIconUIArray[i].SetElementSprite(elementSprite);
            _elementIconUIArray[i].Show();
        }

        for(int i= PElement.PlayerElementSlotArray.Length; i< _elementIconUIArray.Length; i++)
        {
            _elementIconUIArray[i].Hide();
        }
    }

   
}
