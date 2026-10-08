using UnityEngine;

public static class ScreenBounds
{
    static Camera cam;

    static Camera Cam
    {
        get
        {
            if (cam == null) cam = Camera.main;
            return cam;
        }
    }

    public static float Top { get { return Cam.transform.position.y + Cam.orthographicSize; } }
    public static float Bottom { get { return Cam.transform.position.y - Cam.orthographicSize; } }
    public static float Right { get { return Cam.transform.position.x + Cam.orthographicSize * Cam.aspect; } }
    public static float Left { get { return Cam.transform.position.x - Cam.orthographicSize * Cam.aspect; } }

    public static bool IsOutside(Vector3 p, float margin)
    {
        return p.x < Left - margin || p.x > Right + margin || p.y < Bottom - margin || p.y > Top + margin;
    }
}
