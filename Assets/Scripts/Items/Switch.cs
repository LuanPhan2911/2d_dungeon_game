using System;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{


    [SerializeField] private Sprite _offSwitchSprite;
    [SerializeField] private Sprite _onSwitchSprite;
    [SerializeField] private AudioClip _switchSound;

    [SerializeField] private float _delayTime = 1f;


    private SpriteRenderer _currentSprite;
    private float _delayCounter = 0f;

    [SerializeField] private InteractUI _interactUI;

 


    public bool _isOn = false;
    public bool CanInteract;

    public UnityEvent<bool> OnSwitchChanged;
    private void Awake()
    {
        _currentSprite = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        _isOn = false;
        OnSwitchChanged.Invoke(_isOn);
    }

    private void Update()
    {
        if (_delayCounter > 0)
        {
            _delayCounter -= Time.deltaTime;
        }

        if (CanInteract && _delayCounter <= 0)
        {
            if (GameInputManager.Instance.PlayerMoveAction.WasPressedThisFrame() &&
                GameInputManager.Instance.IsUpPressed())
            {
                _delayCounter = _delayTime;
                _isOn = !_isOn;
                _currentSprite.sprite = _isOn ? _onSwitchSprite : _offSwitchSprite;
                OnSwitchChanged.Invoke(_isOn);
                AudioManager.Instance.Play(_switchSound, transform.position);
            }

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            CanInteract = true;
            _interactUI.Show();
            _interactUI.SetInteractText(GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.UpBinding));

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            CanInteract = false;
            _interactUI.Hide();

        }
    }


}
