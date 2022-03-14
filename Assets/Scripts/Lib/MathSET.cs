using UnityEngine;

public static class MathSET
{
    public static float AngleBetween(Vector3 x, Vector3 y)
    {
        Vector3 diff = x - y;
        diff.Normalize();

        // 3D
        return Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg - 90;
    }

    public static float Map(float n, float min, float max, float newMin, float newMax)
    {
        return ((n - min) / (max - min)) * (newMax - newMin) + newMin;
    }
}
