using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private bool _isOn = false;

    [SerializeField] private Vector2 _startOffset;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _distance;

    [SerializeField] private LaserBurst _burst;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
   

    public void Toggle(bool isOn)
    {
        
        _isOn = isOn;
        _lineRenderer.enabled = isOn;
    }

    private void Update()
    {
        if (!_isOn)
        {
            _burst.Hide();
            return;
        }

        Vector2 startPoint = (Vector2)transform.position + _startOffset;
        Vector2 endPoint = startPoint + _direction * _distance;

        Debug.Log($"Laser start point: {startPoint}, end point: {endPoint}");   

        RaycastHit2D hit = Physics2D.Raycast(startPoint, _direction, _distance);

        Debug.Log($"Raycast hit: {hit.collider?.name ?? "None"} at point: {hit.point}");
        if (hit.collider != null && !hit.collider.isTrigger)
        {
            endPoint = hit.point;
            _burst.Show();
            _burst.SetLocalPosition(transform.InverseTransformPoint(endPoint));

            if (hit.collider.TryGetComponent(out ITakeLaserDamagable takeDamage))
            {
                takeDamage.TakeLaserDamage();

            }
          
        }
        else
        {
            _burst.Hide();
        }



        _lineRenderer.SetPosition(0, startPoint);
        _lineRenderer.SetPosition(1, endPoint);
    }
}
