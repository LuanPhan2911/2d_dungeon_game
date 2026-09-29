using System;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{



    [Header("Element")]
    [SerializeField] private float _elementSwapCooldown = 0.5f;
    [SerializeField] private ElementData _physicElementType;
    public PlayerElementData[] PlayerElementData;

    public PlayerElementData CurrentPlayerElement { get; private set; }
    public ElementData CurrentElementType => CurrentPlayerElement.element;
   

    public event Action OnElementSwapped;
    public bool IsElementSwapCooldown => _elementSwapTimer > 0f;
    private float _elementSwapTimer;

    public static PlayerElement Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _elementSwapTimer = Mathf.Max(0, _elementSwapTimer - Time.deltaTime);

        if (!IsElementSwapCooldown)
        {
            if (GameInputManager.Instance.PlayerActions.SwapElement1.WasPressedThisFrame())
            {
                SwapElement(0);
            }
            if (GameInputManager.Instance.PlayerActions.SwapElement2.WasPressedThisFrame())
            {
                SwapElement(1);
            }
            if (GameInputManager.Instance.PlayerActions.SwapElement3.WasPressedThisFrame())
            {
                SwapElement(2);
            }
        }

        
    }
    public ElementData GetElementDamage()
    {
        if (CurrentPlayerElement == null || !PlayerSkill.Instance.IsInfusedElementToWeapon)
        {
            return _physicElementType;
        }
        return CurrentElementType;
    }

    public void SetCurrentPlayerElement(PlayerElementData playerElement)
    {
        CurrentPlayerElement = playerElement;
        _elementSwapTimer = _elementSwapCooldown;
        PlayerSkill.Instance.StopInfuseElementToWeapon();
        OnElementSwapped?.Invoke();
    }
    private void SwapElement(int elementIndex)
    {
        if (PlayerElementData.Length <= elementIndex) return;
        PlayerElementData playerElement = PlayerElementData[elementIndex];

        if (CurrentPlayerElement == playerElement) return;


        SetCurrentPlayerElement(playerElement);
    }



    
}
