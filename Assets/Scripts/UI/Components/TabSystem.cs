using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    [SerializeField] private bool _isHorizontalTab = false;
    [SerializeField] private int _defaultShowTabIndex = -1;


    [SerializeField] private TabButton[] _tabButtonArray;

    [SerializeField] private TabContent[] _tabContentArray;
   
    private int _currentTabIndex;
    public int CurrentTabIndex => _currentTabIndex;
    public bool HasDefaultShowTab => _defaultShowTabIndex != -1;

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
        if (HasDefaultShowTab)
        {
            _currentTabIndex = _defaultShowTabIndex;
        }
        OnTabSelected(_currentTabIndex);
    }

    public void NextTab()
    {
        if (_currentTabIndex < _tabContentArray.Length-1)
        {
            int index = _currentTabIndex + 1;
            OnTabSelected(index);
            SetSelectedTabButton(index);
        }
    }
    public void PrevTab()
    {
        if (_currentTabIndex > 0)
        {
            int index = _currentTabIndex - 1;
            OnTabSelected(index);
            SetSelectedTabButton(index);
        }
    }
   
    public void SetSelectedTabButton(int tabIndex)
    {
        if(EventSystem.current!= null && 
            EventSystem.current.currentSelectedGameObject!= _tabButtonArray[tabIndex].gameObject)
        {
            EventSystem.current.SetSelectedGameObject(_tabButtonArray[tabIndex].gameObject);
        }
    }

    public void OnTabSelected(int tabIndex)
    {
     
        for (int i=0; i< _tabContentArray.Length; i++)
        {
            if(i== tabIndex)
            {
                _currentTabIndex = tabIndex;
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
        _tabContentArray[index].SetFirstSelectedGameObject();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        _currentTabIndex = _defaultShowTabIndex;
        gameObject.SetActive(false);
    }

}
