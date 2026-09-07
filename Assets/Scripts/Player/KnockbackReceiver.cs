using System.Collections;
using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{

    private Rigidbody2D _rb;
    public bool IsKnockbacked { get; private set; }



    private Coroutine _knockbackCoroutine;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    
    public void Knockback(Vector2 direction, float force, float duration)
    {
        if(_knockbackCoroutine!= null)
        {
            StopCoroutine( _knockbackCoroutine );
        }
        _knockbackCoroutine= StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {

        IsKnockbacked = true;
        _rb.linearVelocity = Vector2.zero; // Reset current velocity

        _rb.AddForce(direction * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        ResetKnockback();

    }


    private void ResetKnockback()
    {
        IsKnockbacked = false;
        _knockbackCoroutine = null;
    }
}
