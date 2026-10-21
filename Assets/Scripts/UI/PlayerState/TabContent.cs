using UnityEngine;
using UnityEngine.EventSystems;

public class TabContent : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectedGameObject;



    
    
    public void Show()
    {
        gameObject.SetActive(true);
        if (EventSystem.current != null && _firstSelectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectedGameObject);
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
