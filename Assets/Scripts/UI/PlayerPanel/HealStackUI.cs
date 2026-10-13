using UnityEngine;
using UnityEngine.UI;

public class HealStackUI : MonoBehaviour
{
    [SerializeField] private Image _activeImage;



    public void Use()
    {
        _activeImage.gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
