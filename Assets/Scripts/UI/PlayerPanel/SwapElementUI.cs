using UnityEngine;
using static PlayerElement;

public class SwapElementUI : MonoBehaviour
{


    [SerializeField] private SelectElementButton _elementButtonPrefab;


    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        foreach (Transform child in transform)
        {
            if(child.TryGetComponent(out SelectElementButton button))
            {
                button.SetInteract(!PlayerElement.Instance.IsElementSwapCooldown);
            }

        }
    }

    private void UpdateUI()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);

        }
        int pos = 1;
        foreach (PlayerElementData playerElementData in PlayerElement.Instance.ElementArray)
        {
            SelectElementButton button = Instantiate(_elementButtonPrefab, transform);

            button.SetElement(playerElementData.PlayerElementSO, pos);
            pos++;

        }
    }



}
