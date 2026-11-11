using UnityEngine;

public class CheckPointPanel : Panel
{
    public static CheckPointPanel Instance { get; private set; }


    private void Awake()
    {
        if(Instance!= null  && Instance!= this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Hide();
    }
}
