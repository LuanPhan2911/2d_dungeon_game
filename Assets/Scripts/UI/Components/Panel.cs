using UnityEngine;
using UnityEngine.EventSystems;


public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectGameObject;
    [SerializeField] private Panel _backPanel;
    [SerializeField] private bool _isNeedPauseGame=true;

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
        if(EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectGameObject);
        }
        if (_isNeedPauseGame)
        {
            GameManager.Instance.Pause();
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
