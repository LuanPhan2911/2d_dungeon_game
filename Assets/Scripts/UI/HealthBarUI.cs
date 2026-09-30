using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    [SerializeField] private Image _slowChangeBar;
    [SerializeField] private Image _suddenChangeBar;

    [SerializeField] private float _slowChangeDuration=1;


    [SerializeField] private BaseEnemy _enemy;


    private Coroutine AdjustSlowChangeBarCorountine;





    private void OnEnable()
    {
        _enemy.OnHealthChange += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        _enemy.OnHealthChange -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float ratio)
    {
        _suddenChangeBar.fillAmount = ratio;


        if (AdjustSlowChangeBarCorountine != null)
        {
            StopCoroutine(AdjustSlowChangeBarCorountine);
        }

        AdjustSlowChangeBarCorountine = StartCoroutine(StartAdjustSlowChangeBar());
    }

    private IEnumerator StartAdjustSlowChangeBar()
    {
        float elapse = 0f;
        while(elapse <_slowChangeDuration)
        {
            elapse += Time.deltaTime;
            float t = elapse / _slowChangeDuration;
            _slowChangeBar.fillAmount = Mathf.Lerp(_slowChangeBar.fillAmount, _suddenChangeBar.fillAmount, t);
            yield return null;
        }
        _slowChangeBar.fillAmount = _suddenChangeBar.fillAmount;
        


    }




}
