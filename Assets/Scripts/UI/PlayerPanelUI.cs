    using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanelUI : MonoBehaviour
{


    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private Sprite _redHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;


    [SerializeField] private Image[] _heartImaged;




    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
       
    }

    private void UpdateHealthUI(object sender, int health)
    {
        for (int i = 0; i < _heartImaged.Length; i++)
        {
            if (i < health)
            {
                _heartImaged[i].sprite = _redHeartSprite;
            }
            else
            {
                _heartImaged[i].sprite = _emptyHeartSprite;
            }
        }
    }

    private void UpdateScoreText(object sender, int coin)
    {
        _scoreText.text = $"{coin}";
    }
}
