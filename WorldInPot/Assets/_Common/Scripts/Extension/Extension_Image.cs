using UnityEngine;
using UnityEngine.UI;

public static class Extension_Image
{
	public static float GetHeightFromSpriteAspectRatio(this Image picture, float desiredWidth = default)
	{
		RectTransform ImageRect = picture.GetComponent<RectTransform>();

		float bodyWidth = desiredWidth == default ? ImageRect.rect.width : desiredWidth;
		float imageWidth = picture.sprite.texture.width;
		float imageHeight = picture.sprite.texture.height;
		float ratio = imageWidth / imageHeight;
		float expectedHeight = bodyWidth / ratio;

		return expectedHeight;
	}
}