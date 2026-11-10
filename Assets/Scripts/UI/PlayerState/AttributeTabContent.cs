using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AttributeTabContent : TabContent
{


    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private BreakthroughStar _breakthroughStar;
    [SerializeField] private BaseBar _expBar;
    [SerializeField] private TextMeshProUGUI _maxHpText;
    [SerializeField] private TextMeshProUGUI _atkText;
    [SerializeField] private TextMeshProUGUI _defText;


    [Header("Breakthrough")]
    [SerializeField] private RequireItemBagSlot[] _requireItemBagSlotArray;
  

    public PlayerLevel PLevel => PlayerLevel.Instance;






    private void Start()
    {
        UpdateLevelUI();
        UpdateBreakthrough();
        UpdateExpBar();
        UpdateStatsUI();


        PLevel.OnExpChanged += PLevel_OnExpChanged;
        PLevel.OnLevelChanged += PLevel_OnLevelChanged;
        PLevel.OnBreakthrough += PLevel_OnBreakthrough;
        PLevel.OnStatsChanged += PLevel_OnStatsChanged;

        InventoryManager.Instance.OnInportantItemAdded += Inventory_OnInportantItemAdded;
    }

    private void Inventory_OnInportantItemAdded()
    {
        UpdateRequireBreakthroughItem();
    }

    private void PLevel_OnStatsChanged()
    {
        UpdateStatsUI();
    }

    private void PLevel_OnBreakthrough()
    {
        UpdateBreakthrough();

    }

    private void PLevel_OnLevelChanged()
    {
        UpdateLevelUI();
    }

    private void PLevel_OnExpChanged()
    {
        UpdateExpBar();
    }

    private void UpdateLevelUI()
    {
        _levelText.text = $"{PLevel.Level}";
    }
    private void UpdateBreakthrough()
    {
        _breakthroughStar.Show(PLevel.BreakthroughCount);
        _nameText.text = PLevel.Title;
        UpdateRequireBreakthroughItem();

    }
    private void UpdateRequireBreakthroughItem()
    {
        for (int i = 0; i < PLevel.RequireItemArray.Length; i++)
        {
            _requireItemBagSlotArray[i].Show();
            _requireItemBagSlotArray[i].SetItem(PLevel.RequireItemArray[i]);
            _requireItemBagSlotArray[i].SetAmountText();
        }

        for (int i = PLevel.RequireItemArray.Length; i < _requireItemBagSlotArray.Length; i++)
        {
            _requireItemBagSlotArray[i].Hide();
        }
    }
    private void UpdateExpBar()
    {


        if (PLevel.ExpToLevelUp == 0)
        {
            _expBar.SetFillAmount(1f);
        }
        else
        {
            float ratio = PLevel.CurrentEXP / PLevel.ExpToLevelUp;
            _expBar.SetFillAmount(ratio);
        }
    }

    private void UpdateStatsUI()
    {
        _maxHpText.text = $"{PLevel.HP}";
        _atkText.text = $"{PLevel.ATK}";
        _defText.text = $"{PLevel.DEF}";
    }


}
