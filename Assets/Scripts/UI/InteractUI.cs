using UnityEngine;

public class InteractUI : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI _interactText;


    private void Start()
    {
        Hide();
    }

    public void SetInteractText(string text)
    {
        _interactText.text = text;
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetYPosition(float yPosition)
    {
        Vector3 newPosition = transform.position;
        newPosition.y = yPosition;
        transform.position = newPosition;
    }
    public void SetXPosition(float xPosition)
    {
        Vector3 newPosition = transform.position;
        newPosition.x = xPosition;
        transform.position = newPosition;
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
