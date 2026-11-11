using UnityEngine;

public class RandomScale : MonoBehaviour
{

    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float _speed = 3f;

    private RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }


    private void Update()
    {
        float t = Mathf.PingPong(Time.time * _speed, 1f);

        float scale = Mathf.Lerp(minScale, maxScale, t);

        if(_rectTransform!= null)
        {
            _rectTransform.localScale = Vector3.one * scale;
        }
        else
        {
            transform.localScale = Vector3.one * scale;
        }
       

    }
}
