using UnityEngine;

public class RotateZ : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private float t = 0;

    private void Update()
    {

        t += Time.deltaTime * _speed;

        if (t > 1)
        {
            t = 0f;
        }

        float z = Mathf.Lerp(0f, 180f, t);

        transform.rotation = Quaternion.Euler(0f, 0f, z);
    }


}
