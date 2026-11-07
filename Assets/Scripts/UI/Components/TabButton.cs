using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TabButton : MonoBehaviour, ISelectHandler
{

    [SerializeField] private Button _button;
    private TabSystem _tabSystem;
    private int _tabIndex;


    public void SetTabSystem(TabSystem tabSystem, int tabIndex)
    {
        _tabSystem = tabSystem;
        _tabIndex = tabIndex;
    }

    private void Awake()
    {

        _button.onClick.AddListener(() =>
        {
            _tabSystem.OnTabClicked(_tabIndex);
        });
    }
  
    public void OnSelect(BaseEventData eventData)
    {
        _tabSystem.OnTabSelected(_tabIndex);
    }


}
