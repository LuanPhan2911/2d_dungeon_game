using UnityEngine;
using UnityEngine.EventSystems;

public class SelectUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject _selectGameObject;

    

    public void OnDeselect(BaseEventData eventData)
    {
        _selectGameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _selectGameObject.SetActive(true);
    }
}
