using UnityEngine;

public class DamageFlash : MonoBehaviour
{



   
    
    [SerializeField] private Color _flashColor = Color.white;

    [SerializeField] private float _pingPongFlashSpeed = 5f;

    [SerializeField]  private SpriteRenderer _spriteRenderer;
    private Coroutine _flashCorountine;
    private Coroutine _pingPongFlashCorountine;
    private Material _flashMaterial;


 

    private static readonly int FlashColorId = Shader.PropertyToID("_FlashColor");
    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
 
    private void Awake()
    {
        _flashMaterial = _spriteRenderer.material;
    }

    private void Start()
    {
        _flashMaterial.SetColor(FlashColorId, _flashColor);
    }

    public void Flash(float duration)
    {
        if (_flashCorountine != null)
        {
            StopCoroutine(_flashCorountine);

        }
      
        _flashCorountine = StartCoroutine(FlashCoroutine(duration));
    }
    public void PingPongFlash(float duration)
    {
        if (_pingPongFlashCorountine != null)
        {
            StopCoroutine(_pingPongFlashCorountine);

        }
        _pingPongFlashCorountine = StartCoroutine(PingPongFlashCoroutine(duration));
    }

    private System.Collections.IEnumerator FlashCoroutine(float duration)
    {

        float elapsedTime = 0f;

        // set the flash color to the material
        _flashMaterial.SetColor(FlashColorId, _flashColor);

        

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float flashAmount = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // set the flash amount to the material
            _flashMaterial.SetFloat(FlashAmountId, flashAmount);
           

            yield return null;
        }
        _flashMaterial.SetFloat(FlashAmountId, 0);
    }
    private System.Collections.IEnumerator PingPongFlashCoroutine(float duration)
    {

        float elapsedTime = 0f;

        // set the flash color to the material
        _flashMaterial.SetColor(FlashColorId, _flashColor);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float flashAmount = Mathf.PingPong(Time.time * _pingPongFlashSpeed, 1);

            // set the flash amount to the material
            _flashMaterial.SetFloat(FlashAmountId, flashAmount);

            yield return null;
        }
        _flashMaterial.SetFloat(FlashAmountId, 0);
    }


}
