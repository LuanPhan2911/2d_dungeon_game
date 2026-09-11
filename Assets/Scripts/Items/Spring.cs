using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour
{


    [SerializeField] private Sprite _sprungSprite;
    [SerializeField] private float _duration = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private Sprite _originalSprite;
    private AudioSource _audioSource;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalSprite = _spriteRenderer.sprite;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerMovement player))
        {
            _spriteRenderer.sprite = _sprungSprite;
            _audioSource.Play();
            StartCoroutine(ReturnToSpringCoroutine());
        }
    }

    private IEnumerator ReturnToSpringCoroutine()
    {
        yield return new WaitForSeconds(_duration);

        _spriteRenderer.sprite = _originalSprite;
    }



}
