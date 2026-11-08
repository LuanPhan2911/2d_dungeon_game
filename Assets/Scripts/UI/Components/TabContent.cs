using UnityEngine;
using UnityEngine.EventSystems;


public class TabContent : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectedGameObject;


    public void SetFirstSelectedGameObject()
    {
        if (EventSystem.current != null && _firstSelectedGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectedGameObject);
        }
    }
    public void Show()
    {
        gameObject.SetActive(true); 
    }
    public void Hide()
    {

        gameObject.SetActive(false);

    }
}
