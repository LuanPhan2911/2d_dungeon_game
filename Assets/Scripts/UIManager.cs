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
        Panel activePanel= GetActivePanel();
        if (activePanel != null)
        {
            activePanel.Hide();
            if(activePanel.HasBackPanel)
            {
                activePanel.BackPanel();
            }
            return;
        }

        if (GameManager.Instance.IsGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }

    }

    public void ResumeGame()
    {
        _pausePanel.Hide();
        GameManager.Instance.Unpause();
    }
    public void PauseGame()
    {
        _pausePanel.Show();
        GameManager.Instance.Pause();
    }

    private Panel GetActivePanel()
    {
        foreach (var panel in _panelList)
        {
            if (panel.gameObject.activeSelf)
            {
                return panel;
            }
        }
        return null;
    }   




}
