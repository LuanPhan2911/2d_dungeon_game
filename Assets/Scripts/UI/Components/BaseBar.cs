using UnityEngine;
using UnityEngine.UI;

public class BaseBar : MonoBehaviour
{
    [SerializeField] private Image _image;


    public void SetFillAmount(float amount)
    {
        _image.fillAmount = amount;
    }
}
