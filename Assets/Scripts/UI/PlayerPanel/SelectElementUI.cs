using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectElementUI : MonoBehaviour
{

    [SerializeField] private Image _elementImage;
    [SerializeField] private Image _burstImage;

    public PlayerElement PElement => PlayerElement.Instance;

    private PlayerElement.PlayerElementData _data;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

   

    private void Update()
    {
        if (_data== null) return;


        SetElementImage();



        if (PElement.IsElementSwapCooldown)
        {
            _button.interactable = false;
        }
        else
        {
            _button.interactable = true;
        }

        if (PElement.HasBurst(_data) && PElement.ActivePlayerElement!= _data )
        {
            ShowBurstImage();
        }
        else
        {
            HideBurstImage();
        }
    }


    public void SetElement(PlayerElement.PlayerElementData data)
    {
        _data = data;

    }
    private void SetElementImage()
    {
        _elementImage.sprite = _data.PlayerElementSO.element.sprite;
    }
   
    private void ShowBurstImage()
    {
        _burstImage.sprite = _data.PlayerElementSO.burstSprite;
        _burstImage.color = _data.PlayerElementSO.element.color;
        _burstImage.gameObject.SetActive(true);
    }
    private void HideBurstImage()
    {
        _burstImage.gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
