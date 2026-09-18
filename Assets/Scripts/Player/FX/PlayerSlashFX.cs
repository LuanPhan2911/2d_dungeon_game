using UnityEngine;

public class PlayerSlashFX : MonoBehaviour
{
    [SerializeField] private Transform _sideSlash;
    [SerializeField] private Transform _upSlash;
    [SerializeField] private Transform _downSlash;


    private void Start()
    {
        Hide();
    }

    public void Show(Vector2 direction)
    {

        if (direction == Vector2.up)
        {
            _upSlash.gameObject.SetActive(true);
        }
        else if (direction == Vector2.down)
        {
            _downSlash.gameObject.SetActive(true);

        }
        else
        {
            _sideSlash.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        _sideSlash.gameObject.SetActive(false);
        _upSlash.gameObject.SetActive(false);
        _downSlash.gameObject.SetActive(false);
    }
}
