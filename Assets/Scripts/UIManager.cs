using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }



    [SerializeField] private Panel _pausePanel;

    [SerializeField] private List<Panel> _panelList;




    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInputManager.Instance.UIActions.Cancel.performed += CancelPerform;
    }

    private void CancelPerform(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(_panelList.Count > 0)
        {
            Panel topPanel = _panelList[_panelList.Count - 1];  
            topPanel.Hide();
        }
        else
        {
            _pausePanel.Show();
        }

    }

 
    public void PushPanel(Panel panel)
    {
        if(_panelList.Contains(panel) == false)
        {
            _panelList.Add(panel);
        }
    }
    public void PopPanel(Panel panel)
    {
        if (_panelList.Contains(panel))
        {
            _panelList.Remove(panel);
        }
    }




}
