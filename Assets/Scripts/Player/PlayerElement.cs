using System;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{



    [Header("Element")]
    [SerializeField] private float _elementSwapCooldown = 0.5f;
    [SerializeField] private ElementData _defaultElement;
    public ElementData[] Elements;
    public ElementData CurrentElement { get; private set; }
    public ElementData GetElementDamage()
    {
        if (CurrentElement == null || !PlayerAttack.Instance.IsInfusedElementToWeapon)
        {
            return _defaultElement;
        }
        return CurrentElement;
    }

    public void SetCurrentElement(ElementData element)
    {
        CurrentElement = element;
        _elementSwapTimer = _elementSwapCooldown;
        PlayerAttack.Instance.StopInfuseElementToWeapon();
        OnElementSwapped?.Invoke();
    }

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

    private void SwapElement(int elementIndex)
    {
        if (Elements.Length <= elementIndex) return;
        ElementData element = Elements[elementIndex];
        if (CurrentElement == element) return;
        SetCurrentElement(element);
    }



    
}
