using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image _selectImage;

    private void OnDisable()
    {
        _selectImage.enabled = false;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _selectImage.enabled = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _selectImage.enabled = true;
    }
}
