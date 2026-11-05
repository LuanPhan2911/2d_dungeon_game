using System;
using UnityEngine;
using UnityEngine.UI;
public class SettingPanel : Panel
{
    [SerializeField] private TabSystem _tabSystem;


   
    private void Start()
    {
        // default show sound tab
        _tabSystem.OnTabSelected(_tabSystem.CurrentTabIndex);
        Hide();
    }


}
