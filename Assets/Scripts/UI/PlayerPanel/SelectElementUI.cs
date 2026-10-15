using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectElementUI : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMesh;

 

    public void SetElement(PlayerElementSO data, int positionNumber)
    {
     
        _image.sprite = data.element.sprite;
        _textMesh.text = $"{positionNumber}";

        
    }
    public void SetInteract(bool canInteract)
    {
        _button.interactable = canInteract;
    }
}
