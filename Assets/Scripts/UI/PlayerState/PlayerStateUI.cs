using UnityEngine;

public class PlayerStateUI : MonoBehaviour
{
    [SerializeField] private GameObject _backgroundGameObject;

    [SerializeField] private TabSystem _tabSystem;

    public bool IsSubMenuOpened;
    public GameObject SubMenuGameObject;

    const int STATE_TAB_INDEX= 0;
    const int WEAPON_TAB_INDEX = 1;
    const int BAG_TAB_INDEX = 2;
    const int SKILL_TAB_INDEX = 3;
    const int MAP_TAB_INDEX = 4;

    public GameInputManager GameInput => GameInputManager.Instance;

    private void Start()
    {
        GameInput.UIActions.State.performed += State_Performed;
        GameInput.UIActions.Weapon.performed += Weapon_Performed; 
        GameInput.UIActions.Bag.performed += Bag_Performed; 
        GameInput.UIActions.Skill.performed += Skill_Performed; 
        GameInput.UIActions.Map.performed += Map_Performed; 

        GameInput.UIActions.Cancel.performed += Cancel_Performed;
        GameInput.UIActions.Prev.performed += PrevTab_Performed;
        GameInput.UIActions.Next.performed += NextTab_Performed;

        

        Hide();
    }
   
    private void State_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(STATE_TAB_INDEX);
    }
    private void Weapon_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(WEAPON_TAB_INDEX);
    }
    private void Bag_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(BAG_TAB_INDEX);
    }
    private void Skill_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(SKILL_TAB_INDEX);
    }
    private void Map_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(MAP_TAB_INDEX);
    }

    

    private void Cancel_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (IsSubMenuOpened)
        {
            CloseSubMenu();
            return;
        }
        
        Hide();
    }
    private void CloseSubMenu()
    {
        if(SubMenuGameObject!= null)
        {
            SubMenuGameObject.SetActive(false);
            IsSubMenuOpened = false;
        }
    }

    private void PrevTab_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _tabSystem.PrevTab();
    }
    private void NextTab_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _tabSystem.NextTab();
    }



    private void Show(int tabIndex)
    {
        if(_tabSystem.CurrentTabIndex!= -1 && _tabSystem.CurrentTabIndex== tabIndex)
        {
            Hide();
            return;
        }

        Time.timeScale = 0;
        GameInput.PlayerActions.Disable();

        GameInput.EnableControlUI();
      
        _backgroundGameObject.SetActive(true);


        _tabSystem.Show();
        _tabSystem.OnTabSelected(tabIndex);
    }
    private void Hide()
    {
        Time.timeScale = 1f;
        GameInput.PlayerActions.Enable();
        GameInput.DisableControlUI();

        _backgroundGameObject.SetActive(false);
        _tabSystem.Hide();
    }
}
