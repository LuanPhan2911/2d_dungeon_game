using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectElementButton : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMesh;

    private ElementData _elementData;

    
   


    public void SetElement(ElementData data, int positionNumber)
    {
        _elementData = data;
        _image.color = data.color;
        _textMesh.text = $"{positionNumber}";

        
    }
    public void SetInteract(bool canInteract)
    {
        _button.interactable = canInteract;
    }
}
