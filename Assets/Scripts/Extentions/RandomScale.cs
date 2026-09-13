using UnityEngine;

public class RandomScale : MonoBehaviour
{

    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.5f;


    private void Update()
    {
        float t = Mathf.PingPong(Time.time, 1f);

        float scale = Mathf.Lerp(minScale, maxScale, t);

        transform.localScale = Vector3.one * scale;

    }
}
