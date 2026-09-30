using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public Sprite Sprite;
    public bool isStackable = true;
}
