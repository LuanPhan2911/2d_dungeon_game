using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : MonoBehaviour
{


    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMesh;


    public void SetText(string text)
    {
        _textMesh.text = text;
    }
    public void SetImageFill(float amount)
    {
        _image.fillAmount = amount;
    }
}
