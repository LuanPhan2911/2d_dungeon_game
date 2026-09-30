using System;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{

    public static PlayerElement Instance { get; private set; }

    const int NONE_PLAYER_ELEMENT_INDEX = -1;
    const int FIRST_PLAYER_ELEMENT_INDEX = 0;
    const int SECOND_PLAYER_ELEMENT_INDEX = 1;
    const int THURD_PLAYER_ELEMENT_INDEX = 2;

    [Header("Element")]
    [SerializeField] private float _elementSwapCooldown = 0.5f;
    [SerializeField] private ElementData _physicElementType;
    public PlayerElementData[] PlayerElementArray;


    public event Action OnPlayerElementArrayChanged;


    public PlayerElementData ActivePlayerElement => PlayerElementArray[_activePlayerElementIndex];
    public int ActivePlayerElementIndex => _activePlayerElementIndex;
    public bool HasActivePlayerElement => _activePlayerElementIndex != NONE_PLAYER_ELEMENT_INDEX;
    public ElementData ActiveElementType => ActivePlayerElement.element;
   
    public event Action OnElementSwapped;

    public bool IsElementSwapCooldown => _elementSwapTimer > 0f;
    private float _elementSwapTimer;
    private int _activePlayerElementIndex = NONE_PLAYER_ELEMENT_INDEX;



    private void Awake()
    {
        Instance = this;
       

    }
    private void Start()
    {
      
        OnPlayerElementArrayChanged?.Invoke();
    }
    private void Update()
    {
        _elementSwapTimer = Mathf.Max(0, _elementSwapTimer - Time.deltaTime);

        if (!IsElementSwapCooldown)
        {
            if (GameInputManager.Instance.PlayerActions.SwapElement1.WasPressedThisFrame())
            {
                SetActivePlayerElement(FIRST_PLAYER_ELEMENT_INDEX);
            }
            if (GameInputManager.Instance.PlayerActions.SwapElement2.WasPressedThisFrame())
            {
                SetActivePlayerElement(SECOND_PLAYER_ELEMENT_INDEX);
            }
            if (GameInputManager.Instance.PlayerActions.SwapElement3.WasPressedThisFrame())
            {
                SetActivePlayerElement(THURD_PLAYER_ELEMENT_INDEX);
            }
        }

        
    }
    public ElementData GetElementTypeDamage()
    {
        if (_activePlayerElementIndex!= -1 || !PlayerSkill.Instance.IsInfusedElementToWeapon)
        {
            return _physicElementType;
        }
        return ActiveElementType;
    }
    public void SetActivePlayerElement(int index)
    {
        if (index < 0 || index >= PlayerElementArray.Length) return;

        if (index == _activePlayerElementIndex) return;


        _activePlayerElementIndex = index;
        _elementSwapTimer = _elementSwapCooldown;
        PlayerSkill.Instance.StopInfuseElementToWeapon();
        OnElementSwapped?.Invoke();

    } 
}
