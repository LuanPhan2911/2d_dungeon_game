

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class GamePauseUI : MonoBehaviour
{

    [SerializeField] private GameObject _backgroundGameObject;
    [SerializeField] private Panel _pausePanel;
    [SerializeField] private Panel _optionPanel;
    [SerializeField] private Panel _soundPanel;
    [SerializeField] private Panel _controlPanel;

    public InputSystem_Actions.UIActions UIActions => GameInputManager.Instance.UIActions;

    private List<Panel> _pausePanelList = new List<Panel>();





    private void Start()
    {
        _backgroundGameObject.SetActive(false);
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        UIActions.Cancel.performed += GameInput_CancelPerformed;
    }
    private void GameInput_CancelPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
        RemoveCurrentPanel();
       
    }

    private void GameManager_OnGamePaused()
    {
        if (GameManager.Instance.IsGamePaused)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void AddPanel(Panel panel)
    {
        HideCurrentPanel();
        _pausePanelList.Add(panel);
        panel.Show();
        UIActions.Enable();

    }
    private void ShowCurrentPanel()
    {
        if (_pausePanelList.Count > 0)
        {
            _pausePanelList[_pausePanelList.Count - 1].Show();
        }
    }
    private void HideCurrentPanel()
    {
        if (_pausePanelList.Count > 0)
        {
            _pausePanelList[_pausePanelList.Count - 1].Hide();
        }
    }
    private void RemoveCurrentPanel()
    {
       
        if (_pausePanelList.Count > 0)
        {
            _pausePanelList[_pausePanelList.Count - 1].Hide();
            _pausePanelList.RemoveAt(_pausePanelList.Count - 1);
        }

        ShowCurrentPanel();
        if (_pausePanelList.Count == 0)
        {
            GameManager.Instance.Unpause();
            UIActions.Cancel.Disable();
        }

    }

    public void ResumeButtonClick()
    {
        GameManager.Instance.Unpause();
    }
    public void OptionButtonClick()
    {
        AddPanel(_optionPanel);
    }
    public void MainMenuClick()
    {
        Debug.Log("Go to main menu");
    }
    public void SoundButtonClick()
    {
        AddPanel(_soundPanel);
    }
    public void ControlButtonClick()
    {
        AddPanel(_controlPanel);
    }


    public void Show()
    {
       
        gameObject.SetActive(true);
        _backgroundGameObject.SetActive(true);
        AddPanel(_pausePanel);


    }
    public void Hide()
    {
        _backgroundGameObject.SetActive(true);
        gameObject.SetActive(false);
    }

}
