using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private GameObject[] _starGameObjectArray;
    [SerializeField] private Image _expBar;

    [SerializeField] private TextMeshProUGUI _maxHpText;
    [SerializeField] private TextMeshProUGUI _atkText;
    [SerializeField] private TextMeshProUGUI _defText;

    public PlayerLevel PLevel => PlayerLevel.Instance;


    private void Start()
    {
        UpdateLevelUI();
        UpdateBreakthroghStarUI();
        UpdateExpBar();
        UpdateStatsUI();


        PLevel.OnExpChanged += PLevel_OnExpChanged;
        PLevel.OnLevelChanged += PLevel_OnLevelChanged;
        PLevel.OnBreakthrough += PLevel_OnBreakthrough;
        PLevel.OnStatsChanged += PLevel_OnStatsChanged;
    }

    private void PLevel_OnStatsChanged()
    {
        UpdateStatsUI();
    }

    private void PLevel_OnBreakthrough()
    {
        UpdateBreakthroghStarUI();

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
    private void UpdateBreakthroghStarUI()
    {
        for(int i=0; i< PLevel.BreakthroughCount; i++)
        {
            _starGameObjectArray[i].SetActive(true);
        }
        for(int i= PLevel.BreakthroughCount; i< _starGameObjectArray.Length; i++)
        {
            _starGameObjectArray[i].SetActive(false);
        }
    }
    private void UpdateExpBar()
    {
        

        if (PLevel.ExpToLevelUp == 0)
        {
            _expBar.fillAmount = 1f;
        }
        else
        {
            float ratio = PLevel.CurrentEXP / PLevel.ExpToLevelUp;
            _expBar.fillAmount = ratio;
        }
    }

    private void UpdateStatsUI()
    {
        _maxHpText.text = $"{PLevel.HP}";
        _atkText.text = $"{PLevel.ATK}";
        _defText.text = $"{PLevel.DEF}";
    }
}
