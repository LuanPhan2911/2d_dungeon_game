using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Panel : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectGameObject;

    protected virtual void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
      
        EventSystem.current.SetSelectedGameObject(_firstSelectGameObject);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
