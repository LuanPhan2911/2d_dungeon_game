using System;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _backgroundImage;
   public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
        _backgroundImage.sprite = sprite;

    }

    public void SetFillAmount(float fillAmount)
    {
        _image.fillAmount = fillAmount;
    }

}
