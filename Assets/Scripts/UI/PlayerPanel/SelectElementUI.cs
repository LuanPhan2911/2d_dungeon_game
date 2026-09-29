using UnityEngine;

public class SelectElementUI : MonoBehaviour
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
                button.SetInteract(!PlayerAttack.Instance.IsElementChangeCooldown);
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
        foreach(ElementData data in PlayerAttack.Instance.Elements)
        {
           SelectElementButton button= Instantiate(_elementButtonPrefab, transform);

            button.SetElement(data, pos);
            pos++;

        }
    }



}
