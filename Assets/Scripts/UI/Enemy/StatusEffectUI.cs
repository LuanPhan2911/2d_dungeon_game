using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image _image;


   

    public void SetFillAmount(float fillAmount)
    {
        _image.fillAmount = fillAmount;
    }

}
