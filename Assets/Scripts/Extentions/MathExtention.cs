using UnityEngine;

public static class MathExtention 
{
    public static float OneDecimal(this float value)
    {
        return Mathf.Floor(value * 10f) / 10f;
    }
}
