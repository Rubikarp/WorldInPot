using UnityEngine;

public static class Extension_Texture2D
{
    public static Texture2D GetFragment(this Texture2D pImage, Vector2Int pGridSize, Vector2Int pPos)
    {
        pGridSize = Vector2Int.Max(pGridSize, new Vector2Int(1, 1));

        uint lPosX = (uint)Mathf.Clamp(pPos.x, 0, pGridSize.x - 1);
        uint lPosY = (uint)Mathf.Clamp(pPos.y, 0, pGridSize.y - 1);

        // Invert Y axis
        lPosY = (uint)(pGridSize.y - lPosY - 1);

        if (!pImage.isReadable)
        {
            Debug.LogError("Image is not readable");
            return null;
        }

        Texture2D lTexture = new Texture2D(Mathf.CeilToInt(pImage.width / pGridSize.x), Mathf.CeilToInt(pImage.height / pGridSize.y));
        lTexture.SetPixels(pImage.GetPixels((int)(lPosX * lTexture.width), (int)(lPosY * lTexture.height), lTexture.width, lTexture.height));
        lTexture.wrapMode = TextureWrapMode.Clamp;
        lTexture.filterMode = FilterMode.Point;
        lTexture.anisoLevel = 0;
        lTexture.Apply();

        return lTexture;
    }

}