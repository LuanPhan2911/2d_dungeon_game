using UnityEngine;
using UnityEngine.UI;

public class TabSystem : MonoBehaviour
{

    [SerializeField] private Button[] _tabButtonArray;

    [SerializeField] private TabContent[] _contentPanelGameObjectArray;

    private Color _activeTabColor = Color.green;
    private Color _tabColor = Color.white;
    private int _currentTabIndex = -1;

    public int CurrentTabIndex => _currentTabIndex;


    private void Start()
    {
        for(int i=0; i< _tabButtonArray.Length; i++)
        {
            int index = i;
            _tabButtonArray[i].onClick.AddListener(() => OnTabSelected(index));
        }
    }

    public void NextTab()
    {
        if (_currentTabIndex < _contentPanelGameObjectArray.Length-1)
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
        for(int i=0; i< _contentPanelGameObjectArray.Length; i++)
        {
            if(i== index)
            {
                _currentTabIndex = index;
                _tabButtonArray[i].GetComponent<Image>().color = _activeTabColor;
                _contentPanelGameObjectArray[i].Show();
            }
            else
            {
                _tabButtonArray[i].GetComponent<Image>().color = _tabColor;
                _contentPanelGameObjectArray[i].Hide();
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        _currentTabIndex = -1;
        gameObject.SetActive(false);
    }

}
