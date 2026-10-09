using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Scriptable Objects/Element")]



public class ElementSO : ScriptableObject
{
    public string elementName;
    public ElementalType type =ElementalType.None;
    public Color color;


}
public enum ElementalType
{
    None,
    Steel,
    Grass,
    Water,
    Fire,
    Ground

}