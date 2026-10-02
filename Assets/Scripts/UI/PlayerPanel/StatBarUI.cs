using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : MonoBehaviour
{


    [SerializeField] private Image _image;
  
    public void SetImageFill(float amount)
    {
        _image.fillAmount = amount;
    }
}
