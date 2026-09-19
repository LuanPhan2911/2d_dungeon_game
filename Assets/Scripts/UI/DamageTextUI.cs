using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _disappearTime = 1f;

    [SerializeField] private float _fadeoutSpeed = 5f;

    [SerializeField] private float _fontSizeNormal = 64;
    [SerializeField] private float _fontSizeCrit = 96;


    private Color _textColor;

 
    public void SetText(int damgeAmount)
    {

        _textMesh.text = damgeAmount.ToString();
        _textMesh.fontSize = _fontSizeNormal;
        _textColor = _textMesh.color;

        
    }

    private void Update()
    {
        transform.position += new Vector3(0, _moveSpeed * Time.deltaTime, 0);
        _disappearTime -= Time.deltaTime;

        if (_disappearTime < 0)
        {

            _textColor.a -= _fadeoutSpeed * Time.deltaTime;

            if (_textColor.a < 0)
            {

                Destroy(gameObject);
            }

           
        }
    }
}
