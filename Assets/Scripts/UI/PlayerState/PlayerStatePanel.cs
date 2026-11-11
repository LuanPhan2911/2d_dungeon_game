using UnityEngine;

public class PlayerStatePanel : Panel
{
   
    [SerializeField] private TabSystem _tabSystem;

    const int ATTRIBUTE_TAB_INDEX= 0;
    const int CHARM_TAB_INDEX= 1;
    const int BAG_TAB_INDEX = 2;
    const int MAP_TAB_INDEX = 3;

    public GameInputManager GameInput => GameInputManager.Instance;
    private void Start()
    {
        GameInput.UIActions.Attribute.performed += Attribute_Perofrm;
        GameInput.UIActions.Charm.performed += Charm_Perofrm;
        GameInput.UIActions.Bag.performed += Bag_Performed; 
        GameInput.UIActions.Map.performed += Map_Performed; 

        Hide();
    }
   
    private void Attribute_Perofrm(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
       
        Show(ATTRIBUTE_TAB_INDEX);
    }
  
    private void Charm_Perofrm(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(CHARM_TAB_INDEX);
    }
  
    private void Bag_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(BAG_TAB_INDEX);
    }
    private void Map_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Show(MAP_TAB_INDEX);
    }

    private void Show(int tabIndex)
    {
       
        if (gameObject.activeSelf && _tabSystem.CurrentTabIndex == tabIndex)
        {
            Hide();
            return;
        }
        Show();
       
        _tabSystem.OnTabSelected(tabIndex);
        _tabSystem.SetSelectedTabButton(tabIndex);
       

    }
}
