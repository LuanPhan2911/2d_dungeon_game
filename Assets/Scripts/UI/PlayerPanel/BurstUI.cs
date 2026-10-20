using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BurstUI : MonoBehaviour
{
    [SerializeField] private GameObject _burstIconGameObject;
    [SerializeField] private Image _cooldownCircleImage;
    [SerializeField] private GameObject _cooldownGameObject;
    [SerializeField] private Image _energyImage;
    [SerializeField] private Image _mainImage;
    [SerializeField] private TextMeshProUGUI _cooldownText;


    public PlayerElement PElement => PlayerElement.Instance;

    public PlayerElementSlot OnFieldPlayerElementSlot => PElement.GetOnFieldPlayerElementSlot();

    public bool IsBurstCooldown => OnFieldPlayerElementSlot.BurstTimer > 0f;
    public float BurstEnergyRatio =>
        OnFieldPlayerElementSlot.BurstEnergy / OnFieldPlayerElementSlot.PlayerElementType.DataSO.burstEnergy;
    public float BurstCooldownRatio =>
        OnFieldPlayerElementSlot.BurstTimer / OnFieldPlayerElementSlot.PlayerElementType.DataSO.burstCooldown;
    public float BurstCooldownDuration => OnFieldPlayerElementSlot.BurstTimer.OneDecimal();

    private float _currentDuration;

    private void Start()
    {
        PElement.OnFieldPlayerElementSlotChanged += PElement_OnFieldPlayerElementSlotChanged;


        Hide();
    }

    private void PElement_OnFieldPlayerElementSlotChanged()
    {
        if(OnFieldPlayerElementSlot!= null)
        {

            _energyImage.color = OnFieldPlayerElementSlot.PlayerElementType.DataSO.elementSO.color;
            _mainImage.sprite = OnFieldPlayerElementSlot.PlayerElementType.DataSO.burstSprite;
        }
       

        Show();
    }

  

    private void Update()

    {
        if (!PElement.HasPlayerElementSlot || OnFieldPlayerElementSlot== null) return;

        _energyImage.fillAmount = BurstEnergyRatio;

        if (IsBurstCooldown)
        {
            _cooldownCircleImage.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(true);
            _cooldownCircleImage.fillAmount = BurstCooldownRatio;
            _cooldownGameObject.gameObject.SetActive(true);

            if (_currentDuration != BurstCooldownDuration)
            {
                _cooldownText.text = $"{BurstCooldownDuration}";

            }
            _currentDuration = BurstCooldownDuration;
        }
        else
        {
            _cooldownCircleImage.gameObject.SetActive(false);
            _cooldownGameObject.gameObject.SetActive(false);
            _cooldownText.gameObject.SetActive(false);
        }
    }

    private void Show()
    {
        _burstIconGameObject.SetActive(true);
    }

    private void Hide()
    {
        _burstIconGameObject.SetActive(false);
    }

}
