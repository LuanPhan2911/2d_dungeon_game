using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public string description;
    public bool isStackable = true;
    public bool canBeUsed = false;
}
