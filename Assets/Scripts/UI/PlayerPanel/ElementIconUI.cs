using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ElementIconUI : MonoBehaviour
{

    [SerializeField] private Image _elementImage;
    public PlayerElement PElement => PlayerElement.Instance;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

   

    private void Update()
    {
        if (PElement.IsElementSwapCooldown)
        {
            _button.interactable = false;
        }
        else
        {
            _button.interactable = true;
        }

    }


    
    public void SetElementSprite(Sprite sprite)
    {
        _elementImage.sprite = sprite;
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
