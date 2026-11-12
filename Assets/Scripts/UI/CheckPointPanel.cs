using UnityEngine;
using UnityEngine.UI;

public class CheckPointPanel : Panel
{
    public static CheckPointPanel Instance { get; private set; }



    [SerializeField] private Button _restButton;
    [SerializeField] private Button _travelButton;
    [SerializeField] private Button _breakthroughButton;


    private void Awake()
    {
        if(Instance!= null  && Instance!= this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _restButton.onClick.AddListener(RestButtonClick);
        _travelButton.onClick.AddListener(TravelButtonClick);
        _breakthroughButton.onClick.AddListener(BreakthroughButtonClick);
    }
    private void Start()
    {
        Hide();
    }
    public override void Hide()
    {
        GameManager.Instance.Unpause();
        base.Hide();

    }
    public override void Show()
    {
        base.Show();
        GameManager.Instance.Pause();
    }

    private void RestButtonClick()
    {
        Debug.Log("Rest");
    }
    private void TravelButtonClick()
    {
        Debug.Log("Travel");
    }
    private void BreakthroughButtonClick()
    {

        ConfirmationDialog.Instance.Show("Are you sure?", () =>
        {
            if (PlayerLevel.Instance.TryBreakthrough())
            {
                NotificationDialog.Instance.Show("Breakthrough Success", SetFirstSelectedGameObject);
            }
            else
            {
                NotificationDialog.Instance.Show("Breakthrough Fail", SetFirstSelectedGameObject);
            }
           
        }, SetFirstSelectedGameObject);
    }

   
}
