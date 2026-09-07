using UnityEngine;

public class Brick : MonoBehaviour, IDamagable
{

    [SerializeField] private ParticleSystem _brickBreakParticle;
    [SerializeField] private AudioClip _brickBreakSound;



    [SerializeField] private float _destructionTime = 1f;
    private float _takenDamageTime = 0f;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player player))
        {

            Vector2 nornal = collision.contacts[0].normal;

            float dotVal = Vector2.Dot(nornal, Vector2.up);


            if (dotVal > 0.5f)
            {


                DestroySelf();
            }



        }
    }
    private void DestroySelf()
    {
        ParticleSystem particle = Instantiate(_brickBreakParticle, transform.position, Quaternion.identity);
        AudioManager.Instance.Play(_brickBreakSound, transform.position);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        _takenDamageTime += Time.deltaTime;

        float t = _takenDamageTime / _destructionTime;
        float red = Mathf.Lerp(0, 1, t);
        _spriteRenderer.color = new Color(1, 1 - red, 1 - red);

        if (_takenDamageTime > _destructionTime)
        {
            DestroySelf();
        }
    }
}
