using UnityEngine;

public static class SpriteUtils
{
    public static Texture2D Crop(this Texture2D sourceTexture, RectTransform cropRectTransform)
    {
        RectInt cropRect = new(
            Mathf.FloorToInt(cropRectTransform.position.x),
            Mathf.FloorToInt(cropRectTransform.position.y),
            Mathf.FloorToInt(cropRectTransform.rect.width),
            Mathf.FloorToInt(cropRectTransform.rect.height)
        );

        Color[] newPixels = sourceTexture.GetPixels(cropRect.x, cropRect.y, cropRect.width, cropRect.height);
        Texture2D newTexture = new Texture2D(cropRect.width, cropRect.height);
        newTexture.SetPixels(newPixels);
        newTexture.Apply();
        return newTexture;
    }

    public static Texture2D Crop(this Texture2D sourceTexture, Rect rect)
    {
        RectInt cropRect = new(
            Mathf.FloorToInt(rect.position.x),
            Mathf.FloorToInt(rect.position.y),
            Mathf.FloorToInt(rect.width),
            Mathf.FloorToInt(rect.height)
        );

        Color[] newPixels = sourceTexture.GetPixels(cropRect.x, cropRect.y, cropRect.width, cropRect.height);
        Texture2D newTexture = new Texture2D(cropRect.width, cropRect.height);
        newTexture.SetPixels(newPixels);
        newTexture.Apply();
        return newTexture;
    }
}