using UnityEngine;
using UnityEngine.UI;

public class ElementSlotUI : MonoBehaviour
{
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Image _image;
  

    private PlayerElementType _playerElementType;

    public PlayerElementType PlayerElementType => _playerElementType;


    public void SetPlayerElementType(PlayerElementType playerElementType)
    {
        _playerElementType = playerElementType;
    }
   
    public void ToggleSelect()
    {
        if (_playerElementType == null) return;

        if(!_playerElementType.IsElementSlot && PlayerElement.Instance.IsMaxPlayerElementSlot)
        {
            Debug.Log("Max Player Element Slot");
            return;
        }

        if(_playerElementType!= null && _playerElementType.IsActive)
        {
            _playerElementType.IsElementSlot = !_playerElementType.IsElementSlot;

            if (_playerElementType.IsElementSlot)
            {
                // add player element slot
                PlayerElement.Instance.AddPlayerElementSlot(_playerElementType);
            }
            else
            {
                PlayerElement.Instance.RemovePlayerElementSlot(_playerElementType);
                // remove player elemnt slot
            }
        }
     
    }

    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }  
    public void MakeSlotEmpty()
    {
        _image.sprite = _emptySprite;
        _playerElementType = null;
    }

   
}
