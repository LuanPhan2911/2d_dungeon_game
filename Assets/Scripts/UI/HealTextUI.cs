using TMPro;
using UnityEngine;

public class HealTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _disappearTime = 0.5f;


    public void Spawn(float amount)
    {
        _textMesh.text = Mathf.RoundToInt(amount).ToString();
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
