using UnityEngine;
using UnityEngine.EventSystems;


public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectGameObject;
    [SerializeField] private Panel _backPanel;

    public bool HasBackPanel => _backPanel != null;

    private void OnEnable()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.PushPanel(this);
        }   
    }
    private void OnDisable()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.PopPanel(this);
        }
    }

    public virtual void Show()
    {
        
        gameObject.SetActive(true);

        SetFirstSelectedGameObject();

       
    }
    public void SetFirstSelectedGameObject()
    {
        if (EventSystem.current != null && _firstSelectGameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectGameObject);
        }
    }

  
    public virtual void Hide()
    {
        gameObject.SetActive(false);
        if (HasBackPanel)
        {
            _backPanel.Show();
        }
    }
}
