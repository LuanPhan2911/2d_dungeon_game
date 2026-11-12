
public class SettingPanel : Panel
{
    

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

}
