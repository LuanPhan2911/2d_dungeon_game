using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttributePanel : MonoBehaviour
{
    [SerializeField] private PlayerStateUI _playerState;
    [SerializeField] private ElementSlotUI[] _elementSlotArray;

    [SerializeField] private ElementSlotUI[] _elementTypeArray;

    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _defendText;
    [SerializeField] private TextMeshProUGUI _burstDescriptionText;


    private GameObject _lastSelectedSlotGameObject;

    public PlayerElement PElement => PlayerElement.Instance;


    private void Start()
    {
       

        UpdatePlayerElementSlot();
        UpdatePlayerElementType();
        PElement.OnPlayerElementSlotChanged += PElement_OnPlayerElementSlotChanged;
    }
    private void Update()
    {
        GameObject currentSelectedSlotGameObject = EventSystem.current != null ? 
            EventSystem.current.currentSelectedGameObject : null;

        if(currentSelectedSlotGameObject!= _lastSelectedSlotGameObject)
        {
            ShowBurstInfo(currentSelectedSlotGameObject);
            _lastSelectedSlotGameObject = currentSelectedSlotGameObject;
        }
    }

    private void ShowBurstInfo(GameObject currentSelectedSlotGamObject)
    {
    
        _burstDescriptionText.gameObject.SetActive(false);
        if (currentSelectedSlotGamObject!= null&&
            currentSelectedSlotGamObject.TryGetComponent(out ElementSlotUI slot))
        {
            if(slot.PlayerElementType!= null)
            {
             
                _burstDescriptionText.gameObject.SetActive(true);
                _burstDescriptionText.text = slot.PlayerElementType.DataSO.burstDescription;
            }
           
        }
       
    }
   

    private void PElement_OnPlayerElementSlotChanged()
    {
        UpdatePlayerElementType();
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
