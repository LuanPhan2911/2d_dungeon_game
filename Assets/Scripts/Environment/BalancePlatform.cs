using UnityEngine;

public class BalancePlatform : MonoBehaviour
{

    [SerializeField] private float _restoreForce = 10f;

    [SerializeField] private float _damping = 2f;


    private Rigidbody2D _rigidbody2D;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }



    public bool IsPlayerOnTop = false;


    private void FixedUpdate()
    {
        if (!IsPlayerOnTop)
        {
            float currentAngle = Mathf.DeltaAngle(0, transform.eulerAngles.z);

            float torque = -currentAngle * _restoreForce;

            _rigidbody2D.AddTorque(torque);

            _rigidbody2D.angularVelocity = Mathf.Lerp(_rigidbody2D.angularVelocity, 0, _damping * Time.fixedDeltaTime);


        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent(out PlayerMovement player))
        {
            IsPlayerOnTop = true;
          
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerMovement player))
        {
            IsPlayerOnTop = false;
            
        }
    }
}
