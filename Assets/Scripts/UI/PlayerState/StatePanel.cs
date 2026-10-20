using UnityEngine;

public class StatePanel : MonoBehaviour
{
    [SerializeField] private ElementSlotUI[] _elementSlotArray;

    [SerializeField] private ElementSlotUI[] _elementTypeArray;

    public PlayerElement PElement => PlayerElement.Instance;


    private void Start()
    {
        _elementSlotArray[0].SelectSlot();

        UpdatePlayerElementSlot();
        UpdatePlayerElementType();

        PElement.OnPlayerElementTypeSelected += PElement_OnPlayerElementTypeSelected;
        PElement.OnPlayerElementSlotChanged += PElement_OnPlayerElementSlotChanged;
    }

    private void PElement_OnPlayerElementTypeSelected()
    {
        UpdatePlayerElementType();
    }

    private void PElement_OnPlayerElementSlotChanged()
    {
        UpdatePlayerElementSlot();
    }

    private void UpdatePlayerElementType()
    {
        for(int i=0; i< PElement.PlayerElementTypeArray.Length; i++)
        {
            PlayerElementType playerElementType = PElement.PlayerElementTypeArray[i];
            if (playerElementType.IsActive && !playerElementType.IsElementSlot)
            {
                _elementTypeArray[i].SetPlayerElementType(playerElementType);
                _elementTypeArray[i].SetImage(playerElementType.DataSO.elementStoneSprite);
            }
            else
            {
                _elementTypeArray[i].MakeSlotEmpty();
            }
           
        }
    }
    private void UpdatePlayerElementSlot()
    {
        for(int i=0; i< PElement.PlayerElementSlotArray.Length; i++)
        {
            PlayerElementSlot playerElementSlot = PElement.PlayerElementSlotArray[i];
            _elementSlotArray[i].SetPlayerElementType(playerElementSlot.PlayerElementType);
            _elementSlotArray[i].SetImage(playerElementSlot.PlayerElementType.DataSO.elementStoneSprite);
        }
        for(int i = PElement.PlayerElementSlotArray.Length; i< _elementSlotArray.Length; i++)
        {
            _elementSlotArray[i].MakeSlotEmpty();
        }

       
    }
}
