using UnityEngine;

public class PlayerAttack : MonoBehaviour
{


    public int ComboStep = 0;

    

    [SerializeField] private float _comboResetTime = 1.5f;
    [SerializeField] private int _maxComboStep = 4;

    [SerializeField] private int _crouchAttackStep = 4;

    private float _lastClickTime;


    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }


    public void HandleAttack()
    {
        if (_player.IsWallSliding  || _player.IsStopAction) return;

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

        if (_player.IsCrouching)
        {
            ComboStep = _crouchAttackStep;
        }
        else
        {
            ComboStep = ComboStep==_maxComboStep ? 1: ComboStep +1;
        }
        
      

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

        _player.PlayerAnimation.SetAttackCombo(0);

    }
}
