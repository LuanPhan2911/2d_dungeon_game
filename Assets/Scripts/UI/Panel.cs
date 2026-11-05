using UnityEngine;
using UnityEngine.EventSystems;


public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectGameObject;
    [SerializeField] private Panel _backPanel;

    public bool HasBackPanel => _backPanel != null;

    public void Show()
    {
        
        gameObject.SetActive(true);
        if(EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectGameObject);
        }
    }
    public void Hide()
    {
      
        gameObject.SetActive(false);
      
    }
    public void BackPanel()
    {
        Hide();
        _backPanel.Show();
    }
}
