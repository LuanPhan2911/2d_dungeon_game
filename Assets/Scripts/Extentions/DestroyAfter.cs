using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float _duration=0.5f;

    private void Start()
    {
        Destroy(gameObject, _duration);
    }


}
