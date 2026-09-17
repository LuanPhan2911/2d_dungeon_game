using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool IsAttacking;

   


    private PlayerAnimation _playerAnimation;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        HandleAttack();
    }

    public void HandleAttack()
    {
        if (GameInputManager.Instance.PlayerAttackAction.WasPressedThisFrame())
        {
            Debug.Log("Attack");

            if (!IsAttacking)
            {
                Attack();
            }
        }
    }
    private void Attack()
    {
       

        float verticalInput = GameInputManager.Instance.GetVerticalInput();
        if (verticalInput != 0)
        {
            // upward & downward attack
            if(verticalInput > 0)
            {
                IsAttacking = true;
                _playerAnimation.SetTriggerUpwardAttack();
            }else if(verticalInput <0 && _playerMovement.LastOnGroundTime <= 0)
            {
                IsAttacking = true;
                _playerAnimation.SetTriggerDownwardAttack();
            }
        }
        else
        {
            // horizontal attack
            _playerAnimation.SetTriggeHorizontalrAttack();
            IsAttacking = true;


        }
       
        
    }

    public void PerformHitDetection()
    {
        

    }
    public void FinishAttack()
    {
        IsAttacking = false; 
    }
}
