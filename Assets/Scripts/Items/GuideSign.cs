using TMPro;
using UnityEngine;

public class GuideSign :  BaseBuild
{


    [SerializeField] private string _text;

    [SerializeField] private TextMeshProUGUI _textMeshPro;

    public override void UpdateLayout()
    {
        _textMeshPro.text = _text;
    }




}
