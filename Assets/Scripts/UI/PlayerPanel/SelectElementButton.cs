using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectElementButton : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMesh;

 

    public void SetElement(PlayerElementData data, int positionNumber)
    {
     
        _image.color = data.element.color;
        _textMesh.text = $"{positionNumber}";

        
    }
    public void SetInteract(bool canInteract)
    {
        _button.interactable = canInteract;
    }
}
