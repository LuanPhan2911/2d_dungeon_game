using UnityEngine;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{
    [SerializeField] private Button[] _tabButtonArray;

    [SerializeField] private TabContent[] _tabContentArray;

    private int _currentTabIndex = 0;
    public int CurrentTabIndex => _currentTabIndex;


    private void Awake()
    {
        for(int i=0; i< _tabButtonArray.Length; i++)
        {
            int index = i;
            _tabButtonArray[i].onClick.AddListener(() =>
            {
                OnTabSelected(index);
            });
        }
    }

    public void NextTab()
    {
        if (_currentTabIndex < _tabContentArray.Length-1)
        {
            OnTabSelected(_currentTabIndex + 1);
        }
    }
    public void PrevTab()
    {
        if (_currentTabIndex > 0)
        {
            OnTabSelected(_currentTabIndex - 1);
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
