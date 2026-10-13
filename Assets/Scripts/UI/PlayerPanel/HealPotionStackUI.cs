using UnityEngine;

public class HealPotionStackUI : MonoBehaviour
{


    [SerializeField] private HealStackUI[] _healStackArray;

    private PlayerHealth PHealth => PlayerHealth.Instance;


    private void Start()
    {
        UpdateHealPotionStack();
        PHealth.OnHealPotionCountChanged += UpdateHealPotionStack;
    }

    private void UpdateHealPotionStack()
    {
        
        for(int i=0; i< _healStackArray.Length; i++)
        {
            if(i >= PHealth.GetMaxHealPotionCount())
            {
                _healStackArray[i].Hide();
            }else if(i>= PHealth.CurrentHealPotionCount)
            {
                _healStackArray[i].Use();
            }

        }
    }




}
