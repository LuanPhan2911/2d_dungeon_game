using UnityEngine;


public class SwapElementUI : MonoBehaviour
{


    [SerializeField] private SelectElementUI _firstElement;
    [SerializeField] private SelectElementUI _secondElement;

    public PlayerElement PElement => PlayerElement.Instance;

    private void Update()
    {
        _firstElement.SetElement(PElement.FirstPlayerElementData);
        _secondElement.SetElement(PElement.SecondPlayerElementData);

        if (PElement.FirstPlayerElementData!= null)
        {
            _firstElement.Show();  
        }
        else
        {
            _firstElement.Hide();
        }

        if(PElement.SecondPlayerElementData!= null)
        {
            _secondElement.Show(); 
        }
        else
        {
            _secondElement.Hide();
        }
    }
}
