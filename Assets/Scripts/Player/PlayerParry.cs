using System;
using System.Collections;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private float _parryDuration = 0.2f;
    [SerializeField] private float _parryCooldown = 0.5f;
    [SerializeField] private float _dotValue = 0.3f;

   

    public bool IsParrying => _isParrying;

    private bool _isParrying;
    private bool _canParry = true;

    public static event Action OnParrySuccess;

    private void Update()
    {
        if(GameInputManager.Instance.PlayerActions.Parry.WasPressedThisFrame() && _canParry)
        {
            StartCoroutine(TriggerParryWindow());
        }
    }

    private IEnumerator TriggerParryWindow()
    {
        _isParrying = true;
        _canParry = false;

        yield return new WaitForSeconds(_parryDuration);
        _isParrying = false;
        yield return new WaitForSeconds(_parryCooldown);

        _canParry = true;
    }

    public bool CheckParryValid(Vector3 attackPosition)

    {

        if (!_isParrying)
        {
            return false;
        }

        if (!PlayerMovement.Instance.IsGrounded)
        {
            TriggerParrySuccessEvent();
            return true;
        }
        // 1. Xác định hướng nhìn của Player 
        // Nếu bạn dùng cơ chế lật hình bằng transform.localScale.x:
        Vector2 facingDirection = PlayerMovement.Instance.IsFacingRight ? Vector2.right: Vector2.left;

        // (Hoặc nếu bạn dùng xoay transform trục Y, hãy dùng: Vector2 facingDirection = transform.right;)

        // 2. Tính hướng từ Player đến vị trí đòn đánh
        Vector2 directionToAttack = (attackPosition - transform.position).normalized;

        // 3. Tính tích vô hướng (Dot Product)
        float dotProduct = Vector2.Dot(facingDirection, directionToAttack);

        // dotProduct > 0 nghĩa là góc giữa 2 vector nhỏ hơn 90 độ (đang nhìn về phía đòn đánh)
        // Bạn có thể nâng lên góc hẹp hơn (ví dụ > 0.5f tương đương góc dưới 60 độ để tăng độ khó)
       
        if(dotProduct > _dotValue)
        {
            TriggerParrySuccessEvent();
            return true;
        }

        return false;
    }

    private void TriggerParrySuccessEvent()
    {
        OnParrySuccess?.Invoke();
    }
    
}
