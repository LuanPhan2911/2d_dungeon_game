using TMPro;
using UnityEngine;

public class BindingKeyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bindingText;
   
    public void SetBindingText(string text)
    {
        _bindingText.text = text;
    }
}
