using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamagable
{


    [SerializeField] private int _maxHp;
    private int _Hp;

    private void Start()
    {
        _Hp = _maxHp;
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }


     public void TakeDamage(int damage)
    {
        _Hp -= damage;

        if (_Hp <= 0)
        {
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.collider.TryGetComponent(out Player player))
        {


            //player.TakeDamage();

            //Vector2 direction = -collision.GetContact(0).normal;

            //player.TakeKnockback(direction);
            Debug.Log("Player taken damge");
        }
    }


}
