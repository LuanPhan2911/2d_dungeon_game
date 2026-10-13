using TMPro;
using UnityEngine;

public class DamageTextUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _disappearTime = 0.5f;

    [SerializeField] private float _normalDamageSize = 64;
    [SerializeField] private float _critDamageSize = 128;

 
    public void Spawn(Damage damage)
    {

        _textMesh.text = Mathf.RoundToInt(damage.Amount).ToString();
        _textMesh.fontSize = damage.IsCrit ? _critDamageSize: _normalDamageSize;
     
        _textMesh.color = damage.ElementSO.color;
    }

    private void Update()
    {
        transform.position += new Vector3(0, _moveSpeed * Time.deltaTime, 0);
        _disappearTime -= Time.deltaTime;

        if (_disappearTime < 0)
        {

            Destroy(gameObject);


        }
    }
}
