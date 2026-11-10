using UnityEngine;

[CreateAssetMenu(fileName = "BreakthroughSO", menuName = "Scriptable Objects/BreakthroughSO")]
public class BreakthroughSO : ScriptableObject
{
    public string title;
    public InventoryItem[] ItemArray; 
}
