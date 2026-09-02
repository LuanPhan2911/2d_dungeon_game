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
    private float _delayTimer = 0f;

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
        if (_delayTimer > 0)
        {
            _delayTimer -= Time.deltaTime;
        }

        if (CanInteract && _delayTimer <= 0)
        {
            if (GameInputManager.Instance.PlayerMoveAction.WasPressedThisFrame() &&
                GameInputManager.Instance.IsUpPressed())
            {
                _delayTimer = _delayTime;
                _isOn = !_isOn;
                _currentSprite.sprite = _isOn ? _onSwitchSprite : _offSwitchSprite;
                OnSwitchChanged.Invoke(_isOn);
                AudioManager.Instance.Play(_switchSound, transform.position);
            }

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            CanInteract = true;
            _interactUI.Show();
            _interactUI.SetInteractText(GameInputManager.Instance.GetBindingKey(GameInputManager.PlayerBindingAction.UpBinding));

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            CanInteract = false;
            _interactUI.Hide();

        }
    }


}
