using UnityEngine;

public class CommonMethods 
{
    /// <summary>
    /// Returns the angle a vector makes with the positive x-axis in degrees
    /// </summary>
    public static float GetAngleFromVector(Vector2 v)
    {
        float result = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        if (result < 0)
        {
            return result + 360;
        }

        return result;
    }

    /// <summary>
    /// Gets a corresponding normalized Vector2 for a given angle in degrees
    /// </summary>
    public static Vector2 GetVectorFromAngle(float angle)
    {

        Vector2 answer = new Vector2();
        answer.x = Mathf.Cos(angle * Mathf.Deg2Rad);
        answer.y = Mathf.Sin(angle * Mathf.Deg2Rad);
        answer.Normalize();
        return answer;
    }
}
