using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite Sprite;
    public bool isStackable = true;
}
