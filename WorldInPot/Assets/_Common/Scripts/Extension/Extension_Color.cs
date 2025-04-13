using UnityEngine;

public static class Extension_Color
{
    public static Color Alpha(this Color col, float pAlpha)
    {
        return new Color(col.r, col.g, col.b, pAlpha);
    }
}
