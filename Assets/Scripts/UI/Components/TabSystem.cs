using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    [SerializeField] private TabButton[] _tabButtonArray;

    [SerializeField] private TabContent[] _tabContentArray;
    [SerializeField] private bool _isHorizontalTab = false;

    private int _currentTabIndex = 0;
    public int CurrentTabIndex => _currentTabIndex;

    private void OnEnable()
    {
        if(_isHorizontalTab && GameInputManager.Instance!= null)
        {
            GameInputManager.Instance.UIActions.Prev.performed += Prev_Performed;
            GameInputManager.Instance.UIActions.Next.performed += Next_Performed;
        }
    }
    private void OnDisable()
    {
        if (_isHorizontalTab && GameInputManager.Instance != null)
        {
            GameInputManager.Instance.UIActions.Prev.performed -= Prev_Performed;
            GameInputManager.Instance.UIActions.Next.performed -= Next_Performed;
        }
    }

    private void Next_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        NextTab();
    }

    private void Prev_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PrevTab();
    }

   

    private void Awake()
    {
        for(int i=0; i< _tabButtonArray.Length; i++)
        {
            _tabButtonArray[i].SetTabSystem(this, i);
        }
    }
    public virtual void Start()
    {
        OnTabSelected(_currentTabIndex);
    }

    public void NextTab()
    {
        if (_currentTabIndex < _tabContentArray.Length-1)
        {
            int index = _currentTabIndex + 1;
            OnTabSelected(index);
            if(EventSystem.current!= null)
            {
                EventSystem.current.SetSelectedGameObject(_tabButtonArray[index].gameObject);
            }
        }
    }
    public void PrevTab()
    {
        if (_currentTabIndex > 0)
        {
            int index = _currentTabIndex - 1;
            OnTabSelected(index);
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(_tabButtonArray[index].gameObject);
            }
        }
    }

    public void OnTabSelected(int index)
    {
     
        for (int i=0; i< _tabContentArray.Length; i++)
        {
            if(i== index)
            {
                _currentTabIndex = index;
                _tabContentArray[i].Show();
                
            }
            else
            {
                _tabContentArray[i].Hide();
            }
        }
    }
    public void OnTabClicked(int index)
    {
        _tabContentArray[index].SelectFirstSelectedGameObject();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        _currentTabIndex = 0;
        gameObject.SetActive(false);
    }

}
