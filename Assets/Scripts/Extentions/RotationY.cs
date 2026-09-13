using UnityEngine;

public class RotationY : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;


    private float t = 0;

    private void Update()
    {
        // rotation y axis from 0 to 360 degrees with lerp
        t += Time.deltaTime * _speed;

        if (t > 1)
        {
            t = 0f;
        }

        float y = Mathf.Lerp(0f, 180f, t);

        transform.rotation = Quaternion.Euler(0f, y, 0f);
    }
}
