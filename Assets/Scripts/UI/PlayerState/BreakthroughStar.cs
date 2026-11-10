using UnityEngine;

public class BreakthroughStar : MonoBehaviour
{

    [SerializeField] private GameObject[] starGameObjectArray;



    public void Show(int breakThroughCount)
    {
        for(int i=0; i< breakThroughCount; i++)
        {
            starGameObjectArray[i].SetActive(true);
        }
        for(int i= breakThroughCount; i< starGameObjectArray.Length; i++)
        {
            starGameObjectArray[i].SetActive(false);
        }
    }
}
