using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectElementButton : MonoBehaviour
{

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _textMesh;

    private ElementData _data;

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            if(PlayerAttack.Instance.SelectedElement== _data)
            {
                return;
            }
            PlayerAttack.Instance.SetSelectedElement(_data);
        });
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }


    public void SetElement(ElementData data, int positionNumber)
    {
        _data = data;
        _image.color = data.color;
        _textMesh.text = $"{positionNumber}";

        
    }
    public void SetInteract(bool canInteract)
    {
        _button.interactable = canInteract;
    }
}
