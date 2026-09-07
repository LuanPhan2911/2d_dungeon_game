using UnityEngine;

public class PlayerAttack : MonoBehaviour
{


    public int ComboStep = 0;

    

    [SerializeField] private float _comboResetTime = 1.5f;
    [SerializeField] private int _jumpComboStep = 4;
    [SerializeField] private int _groundComboStep = 2;
    [SerializeField] private int _crouchComboStep = 1;



    private float _lastClickTime;
    private int _currentComboStep;


    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }


    public void HandleAttack()
    {
        if (_player.IsWallSliding  || _player.IsStopAction)
        {
            _player.IsAttacking = false;
            ResetCombo();
            return;
        }

        if(Time.time - _lastClickTime > _comboResetTime && !_player.IsAttacking)
        {
            ResetCombo();
        }

        if (GameInputManager.Instance.PlayerAttackAction.WasPressedThisFrame())
        {
            _lastClickTime= Time.time;

            if (!_player.IsAttacking)
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
        _player.IsAttacking = true;
        _player.Rb.linearVelocity = new Vector2(0, _player.Rb.linearVelocityY);

        

        int maxComboStep;
        if (_player.IsCrouching)
        {
            maxComboStep = _crouchComboStep;
        }
        else if (_player.IsGrounded)
        {
            maxComboStep = _groundComboStep;
        }
        else
        {
            maxComboStep = _jumpComboStep;
        }
        if(_currentComboStep != maxComboStep)
        {
            ResetCombo();
        }

        _currentComboStep= maxComboStep;


        ComboStep = ComboStep == maxComboStep ? 1 : ComboStep + 1;


        _player.PlayerAnimation.SetTriggerAttack();
        _player.PlayerAnimation.SetAttackCombo(ComboStep);
        
    }
    public void FinishAttack()
    {
        _player.IsAttacking = false; 
    }

    private void ResetCombo()
    {
        ComboStep = 0;
    }
}
