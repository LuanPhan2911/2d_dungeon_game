using UnityEngine;
using UnityEngine.EventSystems;


public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectGameObject;
    [SerializeField] private Panel _backPanel;
    [SerializeField] private bool _isNeedPauseGame;

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

        if (_isNeedPauseGame)
        {
            GameManager.Instance.Pause();
        }
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

        if (_isNeedPauseGame)
        {
            GameManager.Instance.Unpause();
        }

        if (HasBackPanel)
        {
            _backPanel.Show();
        }


       

    }
}
