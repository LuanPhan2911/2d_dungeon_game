using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class BossStatue : MonoBehaviour
{
    [SerializeField] private Button _firstInteractButton;
    [SerializeField] private Button _restButton;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Button _connectButton;


    private void Start()
    {
        _firstInteractButton.onClick.AddListener(() =>
        {
            Debug.Log("Kich hoat than tuong, mo rong ban do");
        });
        _restButton.onClick.AddListener(() =>
        {
            Debug.Log("Hoi mau, hoi full no");
        });
        _upgradeButton.onClick.AddListener(() =>
        {
            Debug.Log("Hien te cac vat pham thu duoc");
        });
        _connectButton.onClick.AddListener(() =>
        {
            Debug.Log("Cong huong voi nguyen to hien tai o tuong");
        });
    }
}
